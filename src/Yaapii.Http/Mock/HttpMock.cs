//MIT License

//Copyright(c) 2020 ICARUS Consulting GmbH

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

using MockHttpServer;
using System;
using System.Collections.Generic;
using System.Net;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Fake;
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
    public sealed class HttpMock : IScalar<MockServer>, IDisposable
    {
        private readonly IScalar<MockServer> server;
        private readonly IWire wire;

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, string hostname, string path, IWire wire) : this(
            port,
            hostname,
            new Kvp.Of<IWire>(path, wire)
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, string path, IWire wire) : this(
            port,
            "localhost",
            new Kvp.Of<IWire>(path, wire)
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, params IKvp<IWire>[] pathWirePairs) : this(
            port, 
            new Many.Of<IKvp<IWire>>(pathWirePairs),
            "localhost"
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, string hostName, params IKvp<IWire>[] pathWirePairs) : this(
            port,
            new Many.Of<IKvp<IWire>>(pathWirePairs),
            hostName
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, IEnumerable<IKvp<IWire>> pathWirePairs, string hostName = "localhost") : this(
            port,
            new Map.Of<IWire>(pathWirePairs),
            hostName
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, IDictionary<string, IWire> wires, string hostName = "localhost") : this(
            port,
            new FkWire(req =>
            {
                var path = new Path.Of(req).AsString();
                foreach (var kvp in wires)
                {
                    if ($"/{kvp.Key.TrimStart('/')}" == path)
                    {
                        return kvp.Value.Response(req);
                    }
                }
                return new Response.Of(404, $"Path not Found. No wire has been configured for '{path}'.");
            }),
            hostName
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles all incoming requests using the given wire.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, IWire wire, string hostName = "localhost")
        {
            this.wire = wire;
            this.server = 
                new Sticky<MockServer>(() =>
                    new MockServer(
                        port,
                        "{}", 
                        (req, res, prm) => Respond(req, res),
                        hostName
                    )
                );
        }

        public void Dispose()
        {
            this.server.Value().Dispose();
        }

        public MockServer Value()
        {
            return this.server.Value();
        }

        private void Respond(HttpListenerRequest request, HttpListenerResponse response)
        {
            var wireResponse =
                new Response(
                    this.wire,
                    Request(request)
                );
            response.StatusCode = new Status.Of(wireResponse).AsInt();
            response.StatusDescription = new Reason.Of(wireResponse).AsString();
            foreach(var header in new Headers.Of(wireResponse))
            {
                response.Header(header.Key(), header.Value());
            }
            SendBody(response, wireResponse);
        }

        private void SendBody(HttpListenerResponse aspNetResponse, IDictionary<string, string> wireResponse)
        {
            using (var stream = aspNetResponse.OutputStream)
            {
                var formParams = new FormParams.Of(wireResponse);
                if (formParams.Count > 0)
                {
                    using (var body =
                        new InputOf(
                            new Yaapii.Atoms.Text.Joined("&",
                                new Mapped<KeyValuePair<string, string>, string>(kvp =>
                                    $"{kvp.Key}={kvp.Value}",
                                    formParams
                                )
                            )
                        ).Stream()
                    )
                    {
                        body.CopyTo(stream);
                    }
                }
                else if (new Body.Exists(wireResponse).Value())
                {
                    using (var body = new Body.Of(wireResponse).Stream())
                    {
                        body.CopyTo(stream);
                    }
                }
            }
        }

        private IDictionary<string, string> Request(HttpListenerRequest request)
        {
            return
                new Request(
                    new Method(request.HttpMethod.ToLower()),
                    new Address(request.Url),
                    new Headers(request.Headers),
                    new Conditional(
                        () => request.HasEntityBody,
                        new Body(
                            RequestBody(request)
                        )
                    )
                );
        }

        private IText RequestBody(HttpListenerRequest request)
        {
            using (var stream = request.InputStream)
            {
                return
                    new TextOf(
                        new BytesOf(
                            new InputOf(
                                stream
                            )
                        ).AsBytes() // read entire stream before it gets disposed
                    );
            }
        }
    }
}
