using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Uri
{
    public sealed partial class Address : MapInput.Envelope
    {
        /// <summary>
        /// Checks if a request contains the required parts of a <see cref="System.Uri"/>
        /// </summary>
        public sealed class Exists : BooleanEnvelope
        {
            /// <summary>
            /// Checks if a request contains the required parts of a <see cref="System.Uri"/>
            /// </summary>
            public Exists(IDictionary<string, string> input) : base(() => 
                new Scheme.Exists(input).Value() &&
                new Host.Exists(input).Value()
            )
            { }
        }
    }
}
