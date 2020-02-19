namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds an 'Authorization' header field to a request.
    /// </summary>
    public sealed partial class Authorization : HeaderEnvelope
    {
        private const string KEY = "Authorization";

        /// <summary>
        /// Adds an 'Authorization' header field to a request.
        /// </summary>
        public Authorization(string authorization) : base(KEY, authorization)
        { }
    }
}
