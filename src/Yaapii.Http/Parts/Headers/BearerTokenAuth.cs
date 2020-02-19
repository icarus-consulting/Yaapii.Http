namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds an 'Authorization' header field to a request.
    /// Fills it with a bearer token authentication string using the specified token.
    /// </summary>
    public sealed partial class BearerTokenAuth : HeaderEnvelope
    {
        private const string KEY = "Authorization";
        private const string AUTH_PREFIX = "Bearer ";

        /// <summary>
        /// Adds an 'Authorization' header field to a request.
        /// Fills it with a bearer token authentication string using the specified token.
        /// </summary>
        public BearerTokenAuth(string token) : base(() => KEY, () => $"{AUTH_PREFIX}{token}")
        { }
    }
}
