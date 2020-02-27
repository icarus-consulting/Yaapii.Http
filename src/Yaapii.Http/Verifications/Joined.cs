using System.Collections.Generic;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Scalar;

namespace Yaapii.Http.Verifications
{
    /// <summary>
    /// Several verifications joined as one.
    /// </summary>
    public sealed class Joined : VerificationEnvelope
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
