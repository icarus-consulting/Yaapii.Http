using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Body
{
    public sealed partial class FormParam : MapInput.Envelope
    {
        /// <summary>
        /// Gets a form param from a request.
        /// </summary>
        public sealed class Of : TextEnvelope
        {
            /// <summary>
            /// Gets a form param from a request.
            /// </summary>
            public Of(IDictionary<string, string> input, string key) : base(() => input[$"{KEY_PREFIX}{key}"])
            { }
        }
    }
}
