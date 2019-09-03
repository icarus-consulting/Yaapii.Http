using Yaapii.Atoms;

namespace Yaapii.Http.Wire
{
    /// <summary>
    /// Wire with additional assertion on response.
    /// </summary>
    public sealed class VerificationWire : IWire2
    {
        /// <summary>
        /// Original wire.
        /// </summary>
        private readonly IWire2 origin;

        /// <summary>
        /// Assertion.
        /// </summary>
        private readonly IVerification verification;

        public VerificationWire(IWire2 origin, IVerification verification)
        {
            this.origin = origin;
            this.verification = verification;
        }

        public IDict Response(IDict request)
        {
            var response = this.origin.Response(request);
            this.verification.Verify(response);
            return response;

        }
    }
}
