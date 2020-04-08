using System.Collections.Generic;
using Yaapii.Http.Facets;
using Yaapii.JSON;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// To add a json body to a request, use new <see cref="Body"/>(<see cref="IJSON"/> json)
    /// </summary>
    public sealed class JsonBody
    {
        /// <summary>
        /// The body of a request or response as <see cref="IJSON"/>
        /// </summary>
        public sealed class Of : JsonEnvelope
        {
            /// <summary>
            /// The body of a request or response as <see cref="IJSON"/>
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new JSONOf(
                    new Body.Of(input)
                )
            )
            { }
        }
    }
}
