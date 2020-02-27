namespace Yaapii.Http.Wires
{
    /// <summary>
    /// A wire that verifies each response.
    /// </summary>
    public sealed class Verified : WireEnvelope
    {
        /// <summary>
        /// A wire that verifies each response.
        /// </summary>
        public Verified(IWire origin, IVerification verification) : base(request =>
        {
            var response = origin.Response(request);
            verification.Verify(response);
            return response;
        })
        { }
    }
}
