using System.Collections.Generic;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Responses
{
    public sealed partial class Reason
    {
        /// <summary>
        /// Checks if a response has a reason phrase.
        /// </summary>
        public sealed class Exists : BooleanEnvelope
        {
            /// <summary>
            /// Checks if a response has a reason phrase.
            /// </summary>
            public Exists(IDictionary<string, string> input) : base(() => input.Keys.Contains(KEY))
            { }
        }
    }
}
