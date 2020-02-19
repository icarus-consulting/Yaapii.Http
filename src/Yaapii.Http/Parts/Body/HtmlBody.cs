using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts.Body
{
    /// <summary>
    /// Adds a body to a request.
    /// Sets the content type header to text/html.
    /// </summary>
    public sealed class HtmlBody : Envelope
    {
        /// <summary>
        /// Adds a body to a request.
        /// Sets the content type header to text/html.
        /// </summary>
        public HtmlBody(string body) : this(new TextOf(body))
        { }

        /// <summary>
        /// Adds a body to a request.
        /// Sets the content type header to text/html.
        /// </summary>
        public HtmlBody(IText body) : base(
            "text/html",
            body
        )
        { }
    }
}
