﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Text;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Body;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Response;

namespace Yaapii.Http.Wire
{
    /// <summary>
    /// An <see cref="IWire"/> implemented using ASP.NET Core.
    /// </summary>
    public sealed class AspNetCoreWire : IWire
    {
        private readonly TimeSpan timeout;
        private readonly IDictionary<string, HttpMethod> methods;
        private readonly IVerification requestVerification;

        /// <summary>
        /// An <see cref="IWire"/> implemented using ASP.NET Core.
        /// Timeout is 1 minute.
        /// </summary>
        public AspNetCoreWire() : this(new TimeSpan(0, 1, 0))
        { }

        /// <summary>
        /// An <see cref="IWire"/> implemented using ASP.NET Core.
        /// </summary>
        public AspNetCoreWire(TimeSpan timeout)
        {
            this.timeout = timeout;
            this.methods = 
                new Map.Of<HttpMethod>(
                    new KeyValuePair<string, HttpMethod>("delete", HttpMethod.Delete),
                    new KeyValuePair<string, HttpMethod>("get", HttpMethod.Get),
                    new KeyValuePair<string, HttpMethod>("head", HttpMethod.Head),
                    new KeyValuePair<string, HttpMethod>("options", HttpMethod.Options),
                    new KeyValuePair<string, HttpMethod>("post", HttpMethod.Post),
                    new KeyValuePair<string, HttpMethod>("put", HttpMethod.Put),
                    new KeyValuePair<string, HttpMethod>("trace", HttpMethod.Trace)
                );
            this.requestVerification =
                new Verification.Joined(
                    new Verification.Verification(
                        req => new Address.Exists(req).Value(),
                        req => new ArgumentException("No target URI has been specified for the request.")
                    ),
                    new Verification.Verification(
                        req => new Method.Exists(req).Value(),
                        req => new ArgumentException("No http Method has been specified for the request.")
                    ),
                    new Verification.Verification(
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

        public IDictionary<string, string> Response(IDictionary<string, string> request)
        {
            this.requestVerification.Verify(request);
            using (var aspnetClient = AspNetClient(request))
            using (var aspnetResponse = AspNetResponse(aspnetClient, AspNetRequest(request)))
            using (var responseContent = aspnetResponse.Content)
            {
                var body =
                    new BytesOf(
                        new InputOf(
                            responseContent.ReadAsStreamAsync().GetAwaiter().GetResult()
                        )
                    ).AsBytes(); // read stream to end, before responseContent gets disposed
                var response =
                    new Response.Response.Of(
                        new Status((int)aspnetResponse.StatusCode),
                        new Reason(aspnetResponse.ReasonPhrase),
                        new Headers(ResponseHeaders(aspnetResponse)),
                        new Conditional(
                            () => body.Length > 0,
                            new Body(
                                new TextOf(body)
                            )
                        )
                    );
                
                return response;
            }
        }

        private IList<KeyValuePair<string, string>> ResponseHeaders(HttpResponseMessage response)
        {
            var headers = new List<KeyValuePair<string, string>>();
            foreach (var header in response.Headers)
            {
                foreach (var value in header.Value)
                {
                    headers.Add(new KeyValuePair<string, string>(header.Key, value));
                }
            }
            foreach (var header in response.Content.Headers)
            {
                foreach (var value in header.Value)
                {
                    headers.Add(new KeyValuePair<string, string>(header.Key, value));
                }
            }
            return headers;
        }

        private HttpClient AspNetClient(IDictionary<string, string> request)
        {
            var client = new HttpClient();
            client.Timeout = this.timeout;
            foreach (var header in new Headers.Of(request))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key(), header.Value());
            }
            return client;
        }

        private HttpRequestMessage AspNetRequest(IDictionary<string, string> request)
        {
            var headers = new Headers.Of(request);
            var aspnetRequest =
                new HttpRequestMessage(
                    methods[new Method.Of(request).AsString()],
                    new Address.Of(request).Value()
                );
            foreach (var header in headers)
            {
                aspnetRequest.Headers.TryAddWithoutValidation(header.Key(), header.Value());
            }
            var body = new BytesOf(Body(request)).AsBytes();
            if(body.Length > 0)
            {
                aspnetRequest.Content = new ByteArrayContent(body);
                foreach (var header in headers)
                {
                    aspnetRequest.Content.Headers.TryAddWithoutValidation(header.Key(), header.Value());
                }
            }
            return aspnetRequest;
        }

        private HttpResponseMessage AspNetResponse(HttpClient client, HttpRequestMessage request)
        {
            return
                client.SendAsync(
                    request
                ).GetAwaiter().GetResult();
        }

        private string Body(IDictionary<string, string> request)
        {
            var body = "";
            var formParams = new FormParams.Of(request);
            if(formParams.Count > 0)
            {
                body =
                    new Yaapii.Atoms.Text.Joined("&",
                        new Mapped<KeyValuePair<string, string>, string>(kvp =>
                            $"{kvp.Key}={kvp.Value}",
                            formParams
                        )
                    ).AsString();
            }
            else if(new Body.Exists(request).Value())
            {
                body = new Body.Of(request).AsString();
            }
            return body;
        }
    }
}
