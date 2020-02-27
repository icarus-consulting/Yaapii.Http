using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds an 'Authorization' header field to a request.
    /// Fills it with a basic authentication string created from base 64 encoded username and password.
    /// </summary>
    public sealed partial class BasicAuth : HeaderEnvelope
    {
        private const string KEY = "Authorization";
        private const string AUTH_PREFIX = "Basic ";

        /// <summary>
        /// Adds an 'Authorization' header field to a request.
        /// Fills it with a basic authentication string created from base 64 encoded username and password.
        /// </summary>
        public BasicAuth(string user, string pw) : base(
            () => KEY, 
            () => $"{AUTH_PREFIX}{new TextBase64($"{user}:{pw}").AsString()}"
        )
        { }
    }
}
