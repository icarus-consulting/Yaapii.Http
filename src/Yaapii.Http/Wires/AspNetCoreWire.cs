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

using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Text;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Responses;
using Yaapii.Http.Wires.AspNetCore;

namespace Yaapii.Http.Wires
{
    /// <summary>
    /// An <see cref="IWire"/> implemented using ASP.NET Core.
    /// </summary>
    public sealed class AspNetCoreWire : IWire
    {
        private readonly IAspHttpClients clients;
        private readonly TimeSpan timeout;
        private readonly IDictionary<string, HttpMethod> methods;
        private readonly IVerification requestVerification;

        /// <summary>
        /// An <see cref="IWire"/> implemented using ASP.NET Core.
        /// Timeout is 1 minute.
        /// </summary>
        public AspNetCoreWire(IAspHttpClients clients) : this(clients, new TimeSpan(0, 1, 0))
        { }

        /// <summary>
        /// An <see cref="IWire"/> implemented using ASP.NET Core.
        /// </summary>
        public AspNetCoreWire(IAspHttpClients clients, TimeSpan timeout)
        {
            this.clients = clients;
            this.timeout = timeout;
            this.methods =
                new MapOf<HttpMethod>(
                    new KeyValuePair<string, HttpMethod>("delete", HttpMethod.Delete),
                    new KeyValuePair<string, HttpMethod>("get", HttpMethod.Get),
                    new KeyValuePair<string, HttpMethod>("head", HttpMethod.Head),
                    new KeyValuePair<string, HttpMethod>("options", HttpMethod.Options),
                    new KeyValuePair<string, HttpMethod>("post", HttpMethod.Post),
                    new KeyValuePair<string, HttpMethod>("put", HttpMethod.Put),
                    new KeyValuePair<string, HttpMethod>("trace", HttpMethod.Trace)
                );
            this.requestVerification =
                new Verifications.Joined(
                    new Verifications.Verification(
                        req => new Address.Exists(req).Value(),
                        req => new ArgumentException("No target URI has been specified for the request.")
                    ),
                    new Verifications.Verification(
                        req => new Method.Exists(req).Value(),
                        req => new ArgumentException("No http Method has been specified for the request.")
                    ),
                    new Verifications.Verification(
                        req => this.methods.ContainsKey(new Method.Of(req).AsString()),
                        req => new ArgumentException(
                            new Formatted(
                                "Unknown method '{0}'. Known methods are {1}",
                                new Method.Of(req).AsString(),
                                new Yaapii.Atoms.Text.Joined(", ",
                                    this.methods.Keys
                                ).AsString()
                            ).AsString()
                        )
                    )
                );
        }

        public async Task<IDictionary<string, string>> Response(IDictionary<string, string> request)
        {
            this.requestVerification.Verify(request);

            System.Net.ServicePointManager.FindServicePoint(
                new Address.Of(request).Value()
            ).ConnectionLeaseTimeout = (int)this.timeout.TotalMilliseconds; // see  http://byterot.blogspot.com/2016/07/singleton-httpclient-dns.html

            using (var aspnetResponse = AspNetResponse(AspNetRequest(request)))
            using (var responseContent = aspnetResponse.Content)
            using (var responseStream = await responseContent.ReadAsStreamAsync())
            {
                var body =
                    new BytesOf(
                        new InputOf(responseStream)
                    ).AsBytes(); // read stream to end, before it gets disposed
                var response =
                    new Responses.Response.Of(
                        new Status((int)aspnetResponse.StatusCode),
                        new Reason(aspnetResponse.ReasonPhrase),
                        new Headers(ResponseHeaders(aspnetResponse)),
                        new Conditional(
                            () => body.Length > 0,
                            new Body(body)
                        )
                    );

                return response;
            }
        }

        private IList<IKvp> ResponseHeaders(HttpResponseMessage response)
        {
            var headers = new List<IKvp>();
            foreach (var header in response.Headers)
            {
                foreach (var value in header.Value)
                {
                    headers.Add(new KvpOf(header.Key, value));
                }
            }
            foreach (var header in response.Content.Headers)
            {
                foreach (var value in header.Value)
                {
                    headers.Add(new KvpOf(header.Key, value));
                }
            }
            return headers;
        }

        private HttpRequestMessage AspNetRequest(IDictionary<string, string> request)
        {
            var headers = new Headers.Of(request);
            var aspnetRequest =
                new HttpRequestMessage(
                    methods[new Method.Of(request).AsString()],
                    new Address.Of(request).Value()
                );
            foreach (var head in headers)
            {
                aspnetRequest.Headers.TryAddWithoutValidation(head.Key(), head.Value());
            }
            var content = Content(request);
            if (!IsEmpty(content))
            {
                aspnetRequest.Content = content;
                foreach (var head in headers)
                {
                    IEnumerable<string> tryValues = new List<string>();
                    if (
                        !aspnetRequest.Content.Headers.TryGetValues(head.Key(), out tryValues)
                        || !new Contains<string>(tryValues, head.Value()).Value()
                    )
                    {
                        aspnetRequest.Content.Headers.TryAddWithoutValidation(head.Key(), head.Value());
                    }
                }
            }
            return aspnetRequest;
        }

        private HttpResponseMessage AspNetResponse(HttpRequestMessage request)
        {
            return AsyncContext.Run(() => this.clients.Client(this.timeout).SendAsync(request));
        }

        private HttpContent Content(IDictionary<string, string> request)
        {
            HttpContent body = new ByteArrayContent(new EmptyBytes().AsBytes());
            if (new FormParams.Of(request).Keys.Count > 0)
            {
                body = FormContent(request);
            }
            else if (new Body.Exists(request).Value())
            {
                body = BytesContent(request);
            }
            return body;
        }

        private HttpContent FormContent(IDictionary<string, string> request)
        {
            var content = new FormParams.Of(request);
            return
                new FormUrlEncodedContent(
                    new Mapped<string, KeyValuePair<string, string>>(key =>
                        new KeyValuePair<string, string>(key, content[key]),
                        content.Keys
                    )
                );
        }

        private HttpContent BytesContent(IDictionary<string, string> request)
        {
            return
                new ByteArrayContent(
                    new BytesBody.Of(request).AsBytes()
                );
        }

        private bool IsEmpty(HttpContent content)
        {
            return
                content.ReadAsByteArrayAsync()
                .GetAwaiter()
                .GetResult()
                .Length == 0;
        }
    }
}
