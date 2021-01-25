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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Yaapii.Atoms;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Wires.AspNetCore
{
    /// <summary>
    /// Only add one of these to your application to make sure clients will be reused whenever possible.
    /// This will return an existing client, if the same timeout has been used before.
    /// Otherwise, this will create a new client with the given timeout, because the timeout can only be set before the first request is sent.
    /// </summary>
    public sealed class AspNetCoreClients : IAspHttpClients
    {
        private readonly IDictionary<long, HttpClient> clients = new Dictionary<long, HttpClient>();
        private readonly Func<TimeSpan, HttpClient> addClient;
        private readonly IAction setup;

        /// <summary>
        /// Only add one of these to your application to make sure clients will be reused whenever possible.
        /// This will return an existing client, if the same timeout has been used before.
        /// Otherwise, this will create a new client with the given timeout, because the timeout can only be set before the first request is sent.
        /// </summary>
        public AspNetCoreClients() : this(
            SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls
        )
        { }

        /// <summary>
        /// Only add one of these to your application to make sure clients will be reused whenever possible.
        /// This will return an existing client, if the same timeout has been used before.
        /// Otherwise, this will create a new client with the given timeout, because the timeout can only be set before the first request is sent.
        /// </summary>
        public AspNetCoreClients(SecurityProtocolType security) : this(timeout =>
            new HttpClient()
            {
                Timeout = timeout
            },
            new Once(() =>
            {
                ServicePointManager.SecurityProtocol = security;
            })
        )
        { }

        /// <summary>
        /// Only add one of these to your application to make sure clients will be reused whenever possible.
        /// This will return an existing client, if the same timeout has been used before.
        /// Otherwise, this will create a new client with the given timeout, because the timeout can only be set before the first request is sent.
        /// </summary>
        private AspNetCoreClients(Func<TimeSpan, HttpClient> addClient, IAction setup)
        {
            this.addClient = addClient;
            this.setup = setup;
        }

        public HttpClient Client(TimeSpan timeout)
        {
            lock (clients)
            {
                this.setup.Invoke();
                if (!clients.Keys.Contains(timeout.Ticks))
                {
                    clients.Add(
                        timeout.Ticks,
                        this.addClient(timeout)
                    );
                }
            }
            return clients[timeout.Ticks];
        }
    }
}
