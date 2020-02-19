using Yaapii.Atoms;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Parts.Body
{
    /// <summary>
    /// Envelope for adding a body to a request.
    /// </summary>
    public abstract class Envelope : MapInput.Envelope
    {
        private const string KEY = "body";

        /// <summary>
        /// Envelope for adding a body to a request.
        /// </summary>
        protected Envelope(string contentType, IText content) : base(() =>
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
