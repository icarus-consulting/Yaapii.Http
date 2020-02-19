using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts
{
    public sealed partial class Method : MapInput.Envelope
    {
        /// <summary>
        /// Gets the method of a request.
        /// </summary>
        public sealed class Of : TextEnvelope
        {
            /// <summary>
            /// Gets the method of a request.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() => input[KEY])
            { }
        }
    }
}
