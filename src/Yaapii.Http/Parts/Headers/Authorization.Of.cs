using System.Collections.Generic;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Authorization : HeaderEnvelope
    {
        /// <summary>
        /// Gets the values of the 'Authorization' header field from a request.
        /// </summary>
        public sealed class Of : HeaderOfEnvelope
        {
            /// <summary>
            /// Gets the values of the 'Authorization' header field from a request.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(input, KEY)
            { }
        }
    }
}
