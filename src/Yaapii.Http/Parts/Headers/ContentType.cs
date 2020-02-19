namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds a 'Content-Type' header field to a request.
    /// Specifies the media type of the body of the request.
    /// </summary>
    public sealed partial class ContentType : HeaderEnvelope
    {
        private const string KEY = "Content-Type";

        /// <summary>
        /// Adds a 'Content-Type' header field to a request.
        /// Specifies the media type of the body of the request.
        /// </summary>
        public ContentType(string contentType) : base(KEY, contentType)
        { }
    }
}
