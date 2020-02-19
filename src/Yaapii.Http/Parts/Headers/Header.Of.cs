using System.Collections.Generic;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Header : HeaderEnvelope
    {
        /// <summary>
        /// Gets the values of a header field from a request.
        /// </summary>
        public sealed class Of : HeaderOfEnvelope
        {
            /// <summary>
            /// Gets the values of a header field from a request.
            /// </summary>
            public Of(IDictionary<string, string> input, string key) : base(input, key)
            { }
        }
    }
}
