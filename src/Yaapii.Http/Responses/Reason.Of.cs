using System.Collections.Generic;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Responses
{
    public sealed partial class Reason
    {
        /// <summary>
        /// Gets the reason phrase of a response.
        /// </summary>
        public sealed class Of : TextEnvelope
        {
            /// <summary>
            /// Gets the reason phrase of a response.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() => input[KEY])
            { }
        }
    }
}
