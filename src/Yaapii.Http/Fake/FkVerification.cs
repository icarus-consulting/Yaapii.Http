using System;
using System.Collections.Generic;

namespace Yaapii.Http.Fake
{
    /// <summary>
    /// A fake <see cref="IVerification"/> for unit testing.
    /// </summary>
    public sealed class FkVerification : IVerification
    {
        private readonly Action<IDictionary<string, string>> verification;

        /// <summary>
        /// A fake <see cref="IVerification"/> for unit testing.
        /// </summary>
        public FkVerification() : this((dict) => { })
        { }

        /// <summary>
        /// A fake <see cref="IVerification"/> for unit testing.
        /// </summary>
        public FkVerification(Action<IDictionary<string, string>> verification)
        {
            this.verification = verification;
        }

        public void Verify(IDictionary<string, string> input)
        {
            this.verification.Invoke(input);
        }
    }
}
