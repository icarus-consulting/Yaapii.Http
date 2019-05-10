using MockHttpServer;
using System;
using System.Collections.Generic;
using System.Net;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Mock
{
    /// <summary>
    /// <para>
    /// A HTTP server to mock responses.
    /// </para>
    /// <para>
    /// The server works exclusively, by setting a system-wide mutex for the specified port.
    /// </para>
    /// <para>
    /// IMPORTANT: Always put the MockServer into a using Block!
    /// </para>
    /// <code>
    /// using(new HttpMock(9000).With("/test", (rq, rs, pm) => "result"))
    /// {
    ///     //... your code
    /// }
    /// </code>
    /// </summary>
    public sealed class HttpMock : IDisposable
    {
        private readonly IScalar<MockServer> server;

        /// <summary>
        /// <para>
        /// A HTTP server to mock responses.
        /// </para>
        /// <para>
        /// The server works exclusively, by setting a system-wide mutex for the specified port.
        /// </para>
        /// <para>
        /// IMPORTANT: Always put the MockServer into a using Block!
        /// </para>
        /// <code>
        /// using(new HttpMock(9000).With("/test", (rq, rs, pm) => "result"))
        /// {
        ///     //... your code
        /// }
        /// </code>
        /// </summary>
        public HttpMock(int port) : this(new ScalarOf<string>("localhost"), port)
        { }

        /// <summary>
        /// <para>
        /// A HTTP server to mock responses.
        /// </para>
        /// <para>
        /// The server works exclusively, by setting a system-wide mutex for the specified port.
        /// </para>
        /// <para>
        /// IMPORTANT: Always put the MockServer into a using Block!
        /// </para>
        /// <code>
        /// using(new HttpMock(9000).With("/test", (rq, rs, pm) => "result"))
        /// {
        ///     //... your code
        /// }
        /// </code>
        /// </summary>
        public HttpMock(IPAddress ip, int port) : this(
            new ScalarOf<string>(() => ip.ToString()),
            port
        )
        { }

        /// <summary>
        /// <para>
        /// A HTTP server to mock responses.
        /// </para>
        /// <para>
        /// The server works exclusively, by setting a system-wide mutex for the specified port.
        /// </para>
        /// <para>
        /// IMPORTANT: Always put the MockServer into a using Block!
        /// </para>
        /// <code>
        /// using(new HttpMock(9000).With("/test", (rq, rs, pm) => "result"))
        /// {
        ///     //... your code
        /// }
        /// </code>
        /// </summary>
        public HttpMock(string hostname, int port) : this(new ScalarOf<string>(hostname), port)
        { }

        /// <summary>
        /// <para>
        /// A HTTP server to mock responses.
        /// </para>
        /// <para>
        /// The server works exclusively, by setting a system-wide mutex for the specified port.
        /// </para>
        /// <para>
        /// IMPORTANT: Always put the MockServer into a using Block!
        /// </para>
        /// <code>
        /// using(new HttpMock(9000).With("/test", (rq, rs, pm) => "result"))
        /// {
        ///     //... your code
        /// }
        /// </code>
        /// </summary>
        private HttpMock(IScalar<string> hostname, int port)
        {
            this.server = new Solid<MockServer>(() =>
            {
                return 
                    new MockServer(
                        port, 
                        new EnumerableOf<MockHttpHandler>(), 
                        hostname.Value()
                    );
            });
        }

        /// <summary>
        /// Add a url handling to the server.
        /// </summary>
        /// <param name="url">url to handle, e.g. "/test"</param>
        /// <param name="handle">function which will handle a request</param>
        /// <returns>Your specified result.</returns>
        public HttpMock With(string url, Func<HttpListenerRequest, HttpListenerResponse, Dictionary<string, string>, IInput> handle)
        {
            this.server.Value()
                .AddRequestHandler(
                    new MockHttpHandler(url, (req, res, prm) =>
                    {
                        var body = new BytesOf(handle.Invoke(req, res, prm)).AsBytes();
                        res.Content(body);
                    })
                );
            return this;
        }

        /// <summary>
        /// Add a url handling to the server.
        /// </summary>
        /// <param name="url">url to handle, e.g. "/test"</param>
        /// <param name="handle">function which will handle a request</param>
        /// <returns>Your specified result.</returns>
        public HttpMock With(string url, Func<HttpListenerRequest, HttpListenerResponse, Dictionary<string, string>, string> handle)
        {
            this.server.Value().AddRequestHandler(new MockHttpHandler(url, handle));
            return this;
        }

        public void Dispose()
        {
            this.server.Value().Dispose();
        }
    }
}
