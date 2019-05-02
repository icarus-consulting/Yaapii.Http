using System;
using System.Collections.Generic;
using System.Net.Http;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Text;
using Yaapii.Http.Method;
using Yaapii.Http.Response;

namespace Yaapii.Http.Wire
{
    /// <summary>
    /// Can transport your request to the desired address. It is implemented using AspNetCore.
    /// Type is <see cref="IWire"/>
    /// </summary>
    public class ASPNCoreWire : IWire
    {
        private readonly IDictionary<IMethod, HttpMethod> methods;
           
        /// <summary>
        /// Make a new wire.
        /// </summary>
        public ASPNCoreWire()
        {
            methods = 
                new MapOf<IMethod, HttpMethod>(
                    new KeyValuePair<IMethod, HttpMethod>(new Method.Delete(), HttpMethod.Delete),
                    new KeyValuePair<IMethod, HttpMethod>(new Method.Get(), HttpMethod.Get),
                    new KeyValuePair<IMethod, HttpMethod>(new Method.Head(), HttpMethod.Head),
                    new KeyValuePair<IMethod, HttpMethod>(new Method.Options(), HttpMethod.Options),
                    new KeyValuePair<IMethod, HttpMethod>(new Method.Post(), HttpMethod.Post),
                    new KeyValuePair<IMethod, HttpMethod>(new Method.Put(), HttpMethod.Put)
                );
        }

        /// <summary>
        /// Send a request over this wire.
        /// </summary>
        /// <param name="req">The request</param>
        /// <param name="address">The target endpoint</param>
        /// <param name="method">The http verb</param>
        /// <param name="headers">the request headers to send</param>
        /// <param name="body">the request body to send, if needed (use new InputOf(new EmptyBytes()), if you dont need it)</param>
        /// <param name="timeout"></param>
        /// <returns>the response</returns>
        public IResponse Send(IRequest req, Uri address, IMethod method,
            IEnumerable<KeyValuePair<string, string>> headers, IInput body, TimeSpan timeout
        )
        {
            //TODO: Should we make this method async?
            using (HttpClient client = WithTimeout(WithHeaders(new HttpClient(), headers), timeout))
            using (HttpResponseMessage response =
                client.SendAsync(
                    ASPNCoreRequest(method, address, headers, body)
                ).GetAwaiter().GetResult()
            )
            using (HttpContent resContent = response.Content)
            {
                var resHeaders = new List<KeyValuePair<string, string>>();
                foreach (var header in response.Headers) //@TODO: Atomize
                {
                    foreach (var value in header.Value)
                    {
                        resHeaders.Add(new KeyValuePair<string, string>(header.Key, value));
                    }
                }

                var contentStream = resContent.ReadAsStreamAsync().GetAwaiter().GetResult();

                //read bytes fully to return them, since the resContent will be disposed after this method.
                var bytes = 
                    new BytesOf(
                        new InputOf(contentStream)
                    ).AsBytes();

                //Generate response
                var res =
                    new ResponseOf(
                        req,
                        (int)response.StatusCode,
                        new TextOf(
                            response.ReasonPhrase
                        ),
                        resHeaders,
                        new BytesOf(bytes)
                    );

                return res;
            }

        }

        private HttpClient WithHeaders(HttpClient client, IEnumerable<KeyValuePair<string, string>> headers)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
            return client;
        }

        private HttpClient WithTimeout(HttpClient client, TimeSpan timeout)
        {
            client.Timeout = timeout;
            return client;
        }

        private HttpRequestMessage ASPNCoreRequest(IMethod verb, Uri address, IEnumerable<KeyValuePair<string,string>> headers, IInput body)
        {
            var req = new HttpRequestMessage(methods[verb], address.AbsoluteUri);
            foreach (var header in headers)
            {
                req.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            var bytes = new BytesOf(body).AsBytes();
            if (bytes.Length > 0 && verb != new Get())
            {
                req.Content = new ByteArrayContent(bytes);
                foreach (var header in headers)
                {
                    req.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

           

            return req;
        }
    }
}
