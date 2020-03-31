using Newtonsoft.Json.Linq;
using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// Envelope for something returning a json object.
    /// </summary>
    public abstract class JsonEnvelope : IScalar<JToken>
    {
        private readonly IScalar<JToken> json;

        /// <summary>
        /// Envelope for something returning a json object.
        /// </summary>
        protected JsonEnvelope(Func<JToken> json) : this(new Sticky<JToken>(json))
        { }

        /// <summary>
        /// Envelope for something returning a json object.
        /// </summary>
        protected JsonEnvelope(IScalar<JToken> json)
        {
            this.json = json;
        }

        public JToken Value()
        {
            return this.json.Value();
        }
    }
}
