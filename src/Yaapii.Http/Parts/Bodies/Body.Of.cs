using System.Collections.Generic;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class Body
    {
        /// <summary>
        /// Gets the body of a request or response.
        /// </summary>
        public sealed class Of : TextEnvelope
        {
            /// <summary>
            /// Gets the body of a request or response.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() => input[KEY])
            { }
        }
    }
}
