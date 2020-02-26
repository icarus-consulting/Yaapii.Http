using System.Collections.Generic;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class ContentType
    {
        /// <summary>
        /// Gets the values of the 'Content-Type' header field from a request.
        /// Specifies the media type of the body of the request.
        /// </summary>
        public sealed class Of : HeaderOfEnvelope
        {
            /// <summary>
            /// Gets the values of the 'Content-Type' header field from a request.
            /// Specifies the media type of the body of the request.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(input, KEY)
            { }
        }
    }
}
