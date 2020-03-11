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
using System.Net.Http;
using Yaapii.Http.AtomsTemp;

namespace Yaapii.Http.Wires.AspNetCore
{
    /// <summary>
    /// A <see cref="HttpClient"/> with the given timeout.
    /// Clients will be reused, if the same timeout has been used before.
    /// See https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/ for why reusing them is necessary.
    /// Clients won't be reused if a previously unused timeout is needed, because the timeout can only be set before the first request is sent.
    /// </summary>
    public sealed class AspNetCoreClient : IScalar<HttpClient>
    {
        private readonly static IDictionary<long, HttpClient> clients = new Dictionary<long, HttpClient>();
        private readonly TimeSpan timeout;

        /// <summary>
        /// A <see cref="HttpClient"/> with the given timeout.
        /// Clients will be reused, if the same timeout has been used before.
        /// See https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/ for why reusing them is necessary.
        /// Clients won't be reused if a previously unused timeout is needed, because the timeout can only be set before the first request is sent.
        /// </summary>
        public AspNetCoreClient(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        public HttpClient Value()
        {
            lock (clients)
            {
                if(!clients.Keys.Contains(this.timeout.Ticks))
                {
                    clients.Add(
                        this.timeout.Ticks,
                        new HttpClient()
                        {
                            Timeout = this.timeout
                        }
                    );
                }
            }
            return clients[this.timeout.Ticks];
        }
    }
}
