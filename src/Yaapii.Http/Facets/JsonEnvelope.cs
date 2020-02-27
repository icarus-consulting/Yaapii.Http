using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Scalar;
using Yaapii.JSON;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// Envelope for something returning a json object.
    /// </summary>
    public abstract class JsonEnvelope : IJSON
    {
        private readonly IScalar<IJSON> json;

        /// <summary>
        /// Envelope for something returning a json object.
        /// </summary>
        protected JsonEnvelope(Func<IJSON> json) : this(new Sticky<IJSON>(json))
        { }

        /// <summary>
        /// Envelope for something returning a json object.
        /// </summary>
        protected JsonEnvelope(IScalar<IJSON> json)
        {
            this.json = json;
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
