using System.Collections.Generic;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Accept
    {
        /// <summary>
        /// Checks if any "Accept" header field exists.
        /// </summary>
        public sealed class Exists : BooleanEnvelope
        {
            /// <summary>
            /// Checks if any "Accept" header field exists.
            /// </summary>
            public Exists(IDictionary<string, string> input) : base(() =>
                new LengthOf(
                    new Accept.Of(input)
                ).Value() > 0
            )
            { }
        }
    }
}
