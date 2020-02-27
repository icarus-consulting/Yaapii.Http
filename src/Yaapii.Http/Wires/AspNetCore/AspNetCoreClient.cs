using System;
using System.Collections.Generic;
using System.Net.Http;
using Yaapii.Http.AtomsTemp;

namespace Yaapii.Http.Wires.AspNetCore
{
    /// <summary>
    /// Encapsulates a singleton <see cref="HttpClient"/>.
    /// See https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/ for why this is necessary.
    /// </summary>
    public sealed class AspNetCoreClient : IScalar<HttpClient>
    {
        private readonly static IList<HttpClient> client = new List<HttpClient>();
        private readonly TimeSpan timeout;

        /// <summary>
        /// Encapsulates a singleton <see cref="HttpClient"/>.
        /// See https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/ for why this is necessary.
        /// </summary>
        public AspNetCoreClient(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        public HttpClient Value()
        {
            lock (client)
            {
                if(client.Count == 0)
                {
                    client.Add(new HttpClient());
                    client[0].Timeout = this.timeout;
                }
            }
            return client[0];
        }
    }
}
