using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Verification
{
    /// <summary>
    /// Several verifications joined as one.
    /// </summary>
    public sealed class Joined : Envelope
    {
        /// <summary>
        /// Several verifications joined as one.
        /// </summary>
        public Joined(params IVerification[] verifications) : this(new Many.Of<IVerification>(verifications))
        { }

        /// <summary>
        /// Several verifications joined as one.
        /// </summary>
        public Joined(IEnumerable<IVerification> verifications) : base(input =>
            new Each<IVerification>(
                verification => verification.Verify(input),
                verifications
            ).Invoke()
        )
        { }
    }
}
