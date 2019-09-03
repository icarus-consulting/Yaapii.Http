using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Scalar;
using Yaapii.JSON;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http Body as json.
    /// </summary>
    public sealed class JsonBody : DictInputEnvelope
    {
        /// <summary>
        /// Http Body as json.
        /// </summary>
        /// <param name="json"></param>
        public JsonBody(IJSON json) : base(() => 
            new Body(json.ToString())
        )
        { }

        /// <summary>
        /// Http Body as json.
        /// </summary>
        public JsonBody(JToken token) : base(() =>
            new Body(token.ToString())
        )
        { }

        /// <summary>
        /// Json Body from response.
        /// </summary>
        public sealed class Of : IJSON
        {
            private readonly IScalar<IJSON> json;

            public Of(IDict response)
            {
                this.json = 
                    new Sticky<IJSON>(() =>
                        new JSONOf(
                            new Body.Of(response).AsString()
                        )
                    );
            }

            public IJSON Node(string jsonPath)
            {
                return this.json.Value().Node(jsonPath);
            }

            public IList<IJSON> Nodes(string jsonPath)
            {
                return this.json.Value().Nodes(jsonPath);
            }

            public JToken Token()
            {
                return this.json.Value().Token();
            }

            public string Value(string jsonPath)
            {
                return this.json.Value().Value(jsonPath);
            }

            public IList<string> Values(string jsonPath)
            {
                return this.json.Value().Values(jsonPath);
            }
        }
    }
}
