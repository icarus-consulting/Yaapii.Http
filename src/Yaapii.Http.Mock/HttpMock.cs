//MIT License

//Copyright(c) 2023 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Mock.Templates;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;

namespace Yaapii.Http.Mock
{
    /// <summary>
    /// Hosts a local http server for unit testing.
    /// Handles incoming requests using the given wires.
    /// </summary>
    public sealed class HttpMock : IScalar<IWebHost>, IDisposable
    {
        private readonly IScalar<IWebHost> server;

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the first wire template that matches the request.
        /// Always dispose this or the returned <see cref="IWebHost"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, params ITemplate[] templates) : this(
            port,
            new MatchingWire(templates)
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="IWebHost"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, string path, IWire wire) : this(
            port,
            new KvpOf<IWire>(path, wire)
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="IWebHost"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, params IKvp<IWire>[] pathWirePairs) : this(
            port, 
            new ManyOf<IKvp<IWire>>(pathWirePairs)
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="IWebHost"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, IEnumerable<IKvp<IWire>> pathWirePairs) : this(
            port,
            new MapOf<IWire>(pathWirePairs)
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="IWebHost"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, IDictionary<string, IWire> wires) : this(
            port,
            new MatchingWire(wires)
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles all incoming requests using the given wire.
        /// Always dispose this or the returned <see cref="IWebHost"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, IWire wire)
        {
            this.server =
                new ScalarOf<IWebHost>(() =>
                    RunServer(port, wire)
                );
        }

        public void Dispose()
        {
            this.server.Value().Dispose();
        }

        public IWebHost Value()
        {
            return this.server.Value();
        }

        private IWebHost RunServer(int port, IWire wire)
        {
            return
                WebHost.CreateDefaultBuilder()
                    .UseKestrel((opt) =>
                    {
                        opt.ListenAnyIP(port);
                        opt.AllowSynchronousIO = true;
                    })
                    .Configure((app) =>
                        app.Run((httpContext) =>
                            Task.Run(() => Respond(httpContext, wire))
                        )
                    ).Start();
        }

        private void Respond(HttpContext httpContext, IWire wire)
        {
            var response = httpContext.Response;
            IMessage wireResponse;
            try
            {
                wireResponse =
                    wire.Response(
                        Request(httpContext.Request)
                    ).Result;
                WriteHeaders(response, wireResponse); // done inside the try so the stacktrace is written to the response if an invalid header name causes an error
            }
            catch (Exception ex)
            {
                response.Headers.Clear();
                wireResponse =
                    new SimpleMessage(
                        new Status(500),
                        new TextBody(ex.ToString())
                    );
            }
            WriteStatus(response, wireResponse);
            WriteBody(response, wireResponse);
        }

        private void WriteHeaders(HttpResponse aspNetResponse, IMessage wireResponse)
        {
            foreach (var header in new Headers.Of(wireResponse))
            {
                aspNetResponse.Headers.Append(
                    header.Key(),
                    header.Value()
                );
            }
        }

        private void WriteStatus(HttpResponse aspNetResponse, IMessage wireResponse)
        {
            if (new Status.Exists(wireResponse).Value())
            {
                aspNetResponse.StatusCode = new Status.Of(wireResponse).AsInt();
            }

            var responseFeature = aspNetResponse.HttpContext.Features.Get<IHttpResponseFeature>();
            if (
                new Reason.Exists(wireResponse).Value()
                && responseFeature != null // may be null because http/2 doesn't have reason phrases
            )
            {
                responseFeature.ReasonPhrase = new Reason.Of(wireResponse).AsString();
            }
        }

        private void WriteBody(HttpResponse aspNetResponse, IMessage wireResponse)
        {
            using (var outStream = aspNetResponse.Body)
            {
                var formParams = new FormParams.Of(wireResponse);
                if (formParams.Count > 0)
                {
                    new InputOf(
                        new Yaapii.Atoms.Text.Joined("&",
                            new Mapped<KeyValuePair<string, string>, string>(kvp =>
                                $"{kvp.Key}={kvp.Value}",
                                formParams
                            )
                        )
                    ).Stream()
                        .CopyTo(outStream);
                }
                else if (new Body.Exists(wireResponse).Value())
                {
                    var inStream = new Body.Of(wireResponse).Stream();
                    inStream.CopyTo(outStream);
                    inStream.Position = 0;
                }
            }
        }

        private IMessage Request(HttpRequest request)
        {
            return
                new Request(
                    new Method(request.Method.ToLower()),
                    RequestAddress(request),
                    new Headers(
                        RequestHeaders(request)
                    ),
                    new Parts.Conditional(
                        () => (
                                (request.ContentLength ?? 0) > 0
                                || (request.ContentType ?? "") != ""
                            )
                            && !request.HasFormContentType,
                        () => new Body(
                            RequestBody(request)
                        )
                    ),
                    new Parts.Conditional(
                        () => request.HasFormContentType,
                        () => new FormParams(
                            RequestFormParams(request)
                        )
                    )
                );
        }

        private IMessageInput RequestAddress(HttpRequest request)
        {
            return
                new Parts.Joined(
                    new Scheme(request.Scheme.ToLower()),
                    new Parts.Conditional(
                        () => request.Host.HasValue,
                        () => new Host(request.Host.Host)
                    ),
                    new Parts.Conditional(
                        () => request.Host.HasValue && request.Host.Port != null,
                        () => new Port((int)request.Host.Port)
                    ),
                    new Parts.Conditional(
                        () => request.Path.HasValue,
                        () => new Parts.Uri.Path(request.Path.Value)
                    ),
                    new Parts.Conditional(
                        () => request.QueryString.HasValue,
                        () => new QueryParams(request.QueryString.Value)
                    )
                );
        }

        private IEnumerable<IKvp> RequestHeaders(HttpRequest request)
        {
            return
                new Atoms.Enumerable.Joined<IKvp>(
                    new Mapped<string, IEnumerable<IKvp>>((key) =>
                        new Mapped<string, IKvp>((value) =>
                            new KvpOf(key, value),
                            request.Headers[key]
                        ),
                        request.Headers.Keys
                    )
                );
        }

        private IInput RequestBody(HttpRequest request)
        {
            var stream = new MemoryStream();
            request.Body.CopyTo(stream); // read entire stream before it gets disposed
            stream.Position = 0;
            return new InputOf(stream);
        }

        private IDictionary<string, string> RequestFormParams(HttpRequest request)
        {
            return
                new MapOf(
                    new Mapped<string, IKvp>((key) =>
                        new KvpOf(key, request.Form[key]),
                        request.Form.Keys
                    )
                );
        }
    }
}
