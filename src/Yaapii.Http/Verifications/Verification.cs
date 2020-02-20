using System;
using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Error;

namespace Yaapii.Http.Verifications
{
    /// <summary>
    /// Verifies that a request or response fulfills a given condition.
    /// </summary>
    public sealed class Verification : Envelope
    {
        /// <summary>
        /// Verifies that a request or response fulfills a given condition.
        /// </summary>
        public Verification(Func<IDictionary<string, string>, bool> isValid, Func<IDictionary<string, string>, Exception> error) : this(input =>
            new FailWhen(
                !isValid(input),
                error(input)
            )
        )
        { }

        /// <summary>
        /// Verifies that a request or response fulfills a given condition.
        /// </summary>
        public Verification(Func<IDictionary<string, string>, IFail> verify) : this(input => verify(input).Go())
        { }

        /// <summary>
        /// Verifies a request or response.
        /// </summary>
        public Verification(Action<IDictionary<string, string>> verify) : base(verify)
        { }
    }
}
