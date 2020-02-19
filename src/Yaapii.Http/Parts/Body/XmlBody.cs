using Yaapii.Atoms.Text;
using Yaapii.Xml;

namespace Yaapii.Http.Parts.Body
{
    /// <summary>
    /// Adds a body from an <see cref="IXML"/> to a request.
    /// Sets the content type header to application/xml.
    /// </summary>
    public sealed partial class XmlBody : Envelope
    {
        /// <summary>
        /// Adds a body from an <see cref="IXML"/> to a request.
        /// Sets the content type header to application/xml.
        /// </summary>
        public XmlBody(IXML body) : base(
            "application/xml",
            new TextOf(() => body.AsNode().ToString())
        )
        { }
    }
}
