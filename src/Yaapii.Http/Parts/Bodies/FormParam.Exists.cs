using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class FormParam : MapInput.Envelope
    {
        /// <summary>
        /// Checks if a form param exists in a request.
        /// </summary>
        public sealed class Exists : BooleanEnvelope
        {
            /// <summary>
            /// Checks if a form param exists in a request.
            /// </summary>
            public Exists(IDictionary<string, string> input, string key) : base(() => input.Keys.Contains($"{KEY_PREFIX}{key}"))
            { }
        }
    }
}
