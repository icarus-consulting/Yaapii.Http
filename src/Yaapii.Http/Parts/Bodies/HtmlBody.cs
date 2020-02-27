using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a body to a request.
    /// Sets the content type header to text/html.
    /// </summary>
    public sealed class HtmlBody : BodyEnvelope
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
