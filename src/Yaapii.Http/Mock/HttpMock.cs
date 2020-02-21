using MockHttpServer;
using System;
using System.Collections.Generic;
using System.Net;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
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
        public HttpMock(params IKvp<IWire>[] pathWirePairs) : this(
            new Many.Of<IKvp<IWire>>(pathWirePairs),
            0,
            "localhost"
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, params IKvp<IWire>[] pathWirePairs) : this(
            new Many.Of<IKvp<IWire>>(pathWirePairs),
            port, 
            "localhost"
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(string hostName, params IKvp<IWire>[] pathWirePairs) : this(
            new Many.Of<IKvp<IWire>>(pathWirePairs),
            0,
            hostName
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(int port, string hostName, params IKvp<IWire>[] pathWirePairs) : this(
            new Many.Of<IKvp<IWire>>(pathWirePairs),
            port,
            hostName
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(IEnumerable<IKvp<IWire>> pathWirePairs, int port = 0, string hostName = "localhost") : this(
            new Map.Of<IWire>(pathWirePairs),
            port,
            hostName
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles incoming requests using the wire specified for that path.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(IDictionary<string, IWire> wires, int port = 0, string hostName = "localhost") : this(
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
            port,
            hostName
        )
        { }

        /// <summary>
        /// Hosts a local http server for unit testing.
        /// Handles all incoming requests using the given wire.
        /// Always dispose this or the returned <see cref="MockServer"/> after use.
        /// DO NOT use a wire that will send a http request (like AspNetCoreWire), that would just forward incoming requests, potentially causing an infinite loop.
        /// </summary>
        public HttpMock(IWire wire, int port = 0, string hostName = "localhost")
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
            var res =
                new Response(
                    this.wire,
                    Request(request)
                );
            response.StatusCode = new Status.Of(res).AsInt();
            response.StatusDescription = new Reason.Of(res).AsString();
            foreach(var header in new Headers.Of(res))
            {
                response.Header(header.Key(), header.Value());
            }
            var formParams = new FormParams.Of(res);
            if(formParams.Count > 0)
            {
                new InputOf(
                    new Yaapii.Atoms.Text.Joined("&",
                        new Mapped<KeyValuePair<string, string>, string>(kvp =>
                            $"{kvp.Key}={kvp.Value}",
                            formParams
                        )
                    )
                ).Stream().CopyTo(
                    response.OutputStream
                );
            }
            else if(new Body.Exists(res).Value())
            {
                new InputOf(
                    new Body.Of(res)
                ).Stream().CopyTo(
                    response.OutputStream
                );
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
                            new TextOf(
                                new BytesOf(
                                    new InputOf(
                                        request.InputStream
                                    )
                                ).AsBytes() // read entire stream before it gets disposed
                            )
                        )
                    )
                );
        }
    }
}
