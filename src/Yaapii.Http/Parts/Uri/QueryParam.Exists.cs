using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Uri
{
    public sealed partial class QueryParam : MapInput.Envelope
    {
        /// <summary>
        /// Checks if a request contains the specified query parameter.
        /// </summary>
        public sealed class Exists : BooleanEnvelope
        {
            /// <summary>
            /// Checks if a request contains the specified query parameter.
            /// </summary>
            public Exists(IDictionary<string, string> input, string key) : base(() => input.Keys.Contains($"{KEY_PREFIX}{key}"))
            { }
        }
    }
}
