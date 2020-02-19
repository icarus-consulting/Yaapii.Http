using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts
{
    public sealed partial class Method : MapInput.Envelope
    {
        /// <summary>
        /// Checks if the method has been specified for a request.
        /// </summary>
        public sealed class Exists : BooleanEnvelope
        {
            /// <summary>
            /// Checks if the method has been specified for a request.
            /// </summary>
            public Exists(IDictionary<string, string> input) : base(() => input.Keys.Contains(KEY))
            { }
        }
    }
}
