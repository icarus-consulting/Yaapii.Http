namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds a header field to a request.
    /// </summary>
    public sealed partial class Header : HeaderEnvelope
    {
        /// <summary>
        /// Adds a header field to a request.
        /// </summary>
        public Header(string key, string value) : base(key, value)
        { }
    }
}
