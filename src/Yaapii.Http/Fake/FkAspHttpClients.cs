using System;
using System.Net.Http;
using Yaapii.Http.Wires.AspNetCore;

namespace Yaapii.Http.Fake
{
    /// <summary>
    /// Fake <see cref="IAspHttpClients"/> for unit testing.
    /// </summary>
    public sealed class FkAspHttpClients : IAspHttpClients
    {
        private readonly Func<TimeSpan, HttpClient> client;

        /// <summary>
        /// Fake <see cref="IAspHttpClients"/> for unit testing.
        /// </summary>
        public FkAspHttpClients() : this(new HttpClient())
        { }

        /// <summary>
        /// Fake <see cref="IAspHttpClients"/> for unit testing.
        /// </summary>
        public FkAspHttpClients(HttpClient client) : this(timeout => client)
        { }

        /// <summary>
        /// Fake <see cref="IAspHttpClients"/> for unit testing.
        /// </summary>
        public FkAspHttpClients(Func<TimeSpan, HttpClient> client)
        {
            this.client = client;
        }

        public HttpClient Client(TimeSpan timeout)
        {
            return this.client.Invoke(timeout);
        }
    }
}
