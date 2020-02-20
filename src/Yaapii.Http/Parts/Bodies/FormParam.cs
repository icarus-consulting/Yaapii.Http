using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Text;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a form param to q request.
    /// Sets the content type header to application/x-www-form-urlencoded.
    /// </summary>
    public sealed partial class FormParam : MapInput.Envelope
    {
        private const string KEY_PREFIX = "form:";

        /// <summary>
        /// Adds a form param to q request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParam(string key, string value) : base(() =>
            new Joined(
                new ContentType("application/x-www-form-urlencoded"),
                new MapInput.Of(
                    new Kvp.Of(
                        new TextOf(() => $"{KEY_PREFIX}{key}"),
                        value
                    )
                )
            )
        )
        { }
    }
}
