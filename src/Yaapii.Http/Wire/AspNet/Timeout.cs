using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Yaapii.Http.Wire.AspNet
{
    /// <summary>
    /// Timeout for a request.
    /// </summary>
    public sealed class Timeout : IAspContext
    {
        private readonly TimeSpan timeout;

        /// <summary>
        /// Timeout for a request.
        /// </summary>
        public Timeout(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        public void Apply(HttpClient client)
        {
            client.Timeout = this.timeout;
        }
    }
}
