using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Uri
{
    public sealed partial class Port : MapInput.Envelope
    {
        /// <summary>
        /// Checks if the port of a <see cref="System.Uri"/> has been specified for a request.
        /// </summary>
        public sealed class Exists : BooleanEnvelope
        {
            /// <summary>
            /// Checks if the port of a <see cref="System.Uri"/> has been specified for a request.
            /// </summary>
            public Exists(IDictionary<string, string> input) : base(() => input.Keys.Contains(KEY))
            { }
        }
    }
}
