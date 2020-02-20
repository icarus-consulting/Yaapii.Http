using System;
using System.Collections.Generic;

namespace Yaapii.Http.Verifications
{
    /// <summary>
    /// Envelope to verify a request or response.
    /// </summary>
    public abstract class Envelope : IVerification
    {
        private readonly Action<IDictionary<string, string>> verify;

        /// <summary>
        /// Envelope to verify a request or response.
        /// </summary>
        protected Envelope(Action<IDictionary<string, string>> verify)
        {
            this.verify = verify;
        }

        public void Verify(IDictionary<string, string> input)
        {
            this.verify(input);
        }
    }
}
