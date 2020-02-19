namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds an 'Accept' header field to a request.
    /// Specifies media type(s) that is/are acceptable as a response.
    /// </summary>
    public sealed partial class Acccept : HeaderEnvelope
    {
        private const string KEY = "Accept";

        /// <summary>
        /// Adds an 'Accept' header field to a request.
        /// Specifies media type(s) that is/are acceptable as a response.
        /// </summary>
        public Acccept(string contentType) : base(KEY, contentType)
        { }
    }
}
