using System.Collections.Generic;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Accept
    {
        /// <summary>
        /// Gets the values of the 'Accept' header field from a request.
        /// Specifies media type(s) that is/are acceptable as a response.
        /// </summary>
        public sealed class Of : HeaderOfEnvelope
        {
            /// <summary>
            /// Gets the values of the 'Accept' header field from a request.
            /// Specifies media type(s) that is/are acceptable as a response.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(input, KEY)
            { }
        }
    }
}
