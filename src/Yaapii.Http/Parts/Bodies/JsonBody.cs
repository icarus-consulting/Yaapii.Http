using Yaapii.Atoms.Text;
using Yaapii.JSON;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a body from an <see cref="IJSON"/> to a request.
    /// Sets the content type header to application/json.
    /// </summary>
    public sealed partial class JsonBody : Envelope
    {
        /// <summary>
        /// Adds a body from an <see cref="IJSON"/> to a request.
        /// Sets the content type header to application/json.
        /// </summary>
        public JsonBody(IJSON body) : base(
            "application/json", 
            new TextOf(() => body.Token().ToString())
        )
        { }
    }
}
