using System.Collections.Generic;
using Yaapii.Http.Facets;
using Yaapii.JSON;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class JsonBody : Envelope
    {
        /// <summary>
        /// Gets the body of a request or response as <see cref="IJSON"/>.
        /// </summary>
        public sealed class Of : JsonEnvelope
        {
            /// <summary>
            /// Gets the body of a request or response as <see cref="IJSON"/>.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() => new JSONOf(input["body"]))
            { }
        }
    }
}
