using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Uri
{
    public sealed partial class Path : MapInput.Envelope
    {
        /// <summary>
        /// Extracts the path of a <see cref="System.Uri"/> from a request.
        /// </summary>
        public sealed class Of : TextEnvelope
        {
            /// <summary>
            /// Extracts the path of a <see cref="System.Uri"/> from a request.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() => input[KEY])
            { }
        }
    }
}
