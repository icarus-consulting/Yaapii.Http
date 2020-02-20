using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a body to a request.
    /// Sets the content type header to text/plain.
    /// </summary>
    public sealed class TextBody : Envelope
    {
        /// <summary>
        /// Adds a body to a request.
        /// Sets the content type header to text/plain.
        /// </summary>
        public TextBody(string body) : this(new TextOf(body))
        { }

        /// <summary>
        /// Adds a body to a request.
        /// Sets the content type header to text/plain.
        /// </summary>
        public TextBody(IText body) : base(
            "text/plain",
            body
        )
        { }
    }
}
