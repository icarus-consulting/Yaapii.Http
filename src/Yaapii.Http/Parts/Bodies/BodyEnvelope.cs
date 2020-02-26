using Yaapii.Atoms;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Envelope for adding a body to a request.
    /// </summary>
    public abstract class BodyEnvelope : MapInput.Envelope
    {
        private const string KEY = "body";

        /// <summary>
        /// Envelope for adding a body to a request.
        /// </summary>
        protected BodyEnvelope(string contentType, IText content) : base(() =>
            new Joined(
                new ContentType(contentType),
                new MapInput.Of(
                    new Kvp.Of(KEY, () => content.AsString())
                )
            )
        )
        { }
    }
}
