using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class JsonBody
    {
        /// <summary>
        /// Gets the body of a request or response as <see cref="JToken"/>.
        /// </summary>
        public sealed class Of : JsonEnvelope
        {
            /// <summary>
            /// Gets the body of a request or response as <see cref="JToken"/>.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() => JObject.Parse(input["body"]))
            { }
        }
    }
}
