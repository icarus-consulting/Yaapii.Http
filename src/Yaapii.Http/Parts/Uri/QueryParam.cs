using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds a query parameter to a request.
    /// </summary>
    public sealed partial class QueryParam : MapInput.Envelope
    {
        private const string KEY_PREFIX = "query:";

        /// <summary>
        /// Adds a query parameter to a request.
        /// </summary>
        public QueryParam(string key, string value) : base(
            new Kvp.Of(
                new TextOf(() => $"{KEY_PREFIX}{key}"),
                value
            )
        )
        { }
    }
}
