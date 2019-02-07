using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Number;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Error;
using Yaapii.Http.RequestUri;
using Yaapii.Http.Wire;

namespace Yaapii.Http
{
    public sealed class HttpRequest : IRequest
    {
        private readonly IMethod method;
        private readonly IBytes body;
        private readonly IDictionary<string, string> headers;
        private readonly IScalar<IDictionary<string, string>> attributes;

        public HttpRequest(IMethod method, Uri url,IBytes body, TimeSpan timeout) : this(method,url,new MapOf<string,string>(),body,timeout)
        { }
        public HttpRequest(IMethod method, Uri uri, IDictionary<string,string> headers, TimeSpan timeout) : this(method, uri, headers, new EmptyBytes(), timeout)
        { }

        public HttpRequest(IMethod method, Uri uri, TimeSpan timeout) : this(method, uri, new MapOf<string, string>(), new EmptyBytes(), timeout)
        { }

        public HttpRequest(
            IMethod method, Uri uri, IDictionary<string, string> headers, IBytes body,
            TimeSpan timeout
        )
        {
            this.headers = headers;
            this.method = method;
            this.body = body;
            this.attributes =
                new StickyScalar<IDictionary<string, string>>(() =>
                     new MapOf<string, string>(
                         new KeyValuePair<string, string>("connect-timeout", timeout.TotalMilliseconds.ToString()),
                         new KeyValuePair<string, string>("uri", uri.AbsoluteUri.ToString())
                     )
                );
        }

        public IRequest Header(string name, string value)
        {
            return
                new HttpRequest(
                    this.method,
                    BaseUri(),
                    new MapOf<string, string>(
                        this.headers,
                        new KeyValuePair<string, string>(name, value)
                    ),
                    this.body,
                    ConnectionTimeout()
                );
        }

        public IRequestBody Body()
        {
            return new RequestBodyOf(this, this.body);
        }

        public IRequest Method(IMethod verb)
        {
            return new HttpRequest(verb, this.BaseUri(), this.headers, this.body, this.ConnectionTimeout());
        }

        public IRequest Timeout(TimeSpan timeout)
        {
            return new HttpRequest(this.method, this.BaseUri(), this.headers, this.body, timeout);
        }

        public IResponse Response()
        {
            return this.FetchedResponse(new InputOf(this.body));
        }

        public IResponse Response(IInput input)
        {
            if (new LengthOf(new InputOf(this.body)).Value() > 0) //@TODO: Test, possible issue is that a reader is placed at the end after LengthOf.
            {
                throw new IllegalStateException("Request body is not empty, use fetch() instead");
            }

            return this.FetchedResponse(input);
        }

        private IResponse FetchedResponse(IInput body)
        {
            var start = DateTime.Now;
            return new ASPNCoreWire().Send(this, BaseUri(), this.method, this.headers, new InputOf(this.body), ConnectionTimeout());
        }

        public IRequestUri Uri()
        {
            return new BaseUri(this, BaseUri());
        }

        private Uri BaseUri()
        {
            return new System.Uri(this.attributes.Value()["uri"]);
        }

        private TimeSpan ConnectionTimeout()
        {
            return new TimeSpan(0, 0, 0, 0, new NumberOf(this.attributes.Value()["connect-timeout"]).AsInt());
        }

        public IRequest Uri(Uri url)
        {
            return new HttpRequest(this.method, url, this.headers, this.body, ConnectionTimeout());
        }
    }
}
