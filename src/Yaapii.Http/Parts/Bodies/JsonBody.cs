using Newtonsoft.Json.Linq;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a body from an <see cref="JToken"/> to a request.
    /// Sets the content type header to application/json.
    /// </summary>
    public sealed partial class JsonBody : BodyEnvelope
    {
        /// <summary>
        /// Adds a body from an <see cref="JToken"/> to a request.
        /// Sets the content type header to application/json.
        /// </summary>
        public JsonBody(JToken body) : base(
            "application/json", 
            new TextOf(() => body.ToString())
        )
        { }
    }
}
