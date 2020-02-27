using System.Collections.Generic;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Uri
{
    public sealed partial class QueryParam
    {
        /// <summary>
        /// Gets the value of the specified query parameter from a request.
        /// </summary>
        public sealed class Of : TextEnvelope
        {
            /// <summary>
            /// Gets the value of the specified query parameter from a request.
            /// </summary>
            public Of(IDictionary<string, string> input, string key) : base(() => input[$"{KEY_PREFIX}{key}"])
            { }
        }
    }
}
