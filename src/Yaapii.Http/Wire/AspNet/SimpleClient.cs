using System;
using System.Collections.Generic;
using System.Net.Http;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Http.Parts;

namespace Yaapii.Http.Wire.AspNet
{
    /// <summary>
    /// A simple asp http client.
    /// </summary>
    public sealed class SimpleClient : IAspClient
    {
        private readonly IEnumerable<IAspContext> contexts;

        public SimpleClient(params IAspContext[] contexts) : this(new EnumerableOf<IAspContext>(contexts))
        { }

        public SimpleClient(IEnumerable<IAspContext> contexts)
        {
            this.contexts = contexts;
        }

        public HttpResponseMessage Response(IDict request)
        {
            using (HttpClient client = new HttpClient())
            {
                foreach (var context in this.contexts)
                {
                    context.Apply(client);
                }
                return
                    client.SendAsync(RequestMessage(request))
                        .GetAwaiter()
                        .GetResult();
            }
        }

        private HttpRequestMessage RequestMessage(IDict request)
        {
            var req = 
                new HttpRequestMessage(
                    new HttpMethod(new Method.Of(request).AsString()),
                    new Parts.RequestUri.Of(request).AsString()
                );

            req.Content =
                new System.Net.Http.StringContent(
                    new Body.Of(request).AsString()
                );

            foreach(var header in 
                new Headers.AsMap(
                    new Headers.Of(request)
                )
            )
            {
                foreach (var value in header.Value)
                {
                    if(!req.Headers.TryAddWithoutValidation(header.Key, value))
                    {
                        throw new ArgumentException($"Cannot set request header {header.Key}={header.Value} for unknown reasons.");
                    }
                }
            }
            return req;
        }
    }
}