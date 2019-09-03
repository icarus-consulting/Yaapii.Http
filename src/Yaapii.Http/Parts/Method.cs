using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http method (verb).
    /// </summary>
    public sealed class Method : DictInputEnvelope
    {
        private const string KEY = "method";

        /// <summary>
        /// Http method (verb).
        /// </summary>
        public Method(string method) : base(
            new KvpOf(Method.KEY, method)
        )
        { }

        /// <summary>
        /// Method from response.
        /// </summary>
        public sealed class Of : IText
        {
            private readonly IDict response;

            public Of(IDict response)
            {
                this.response = response;
            }

            public string AsString()
            {
                return this.response.Content(Method.KEY, "GET");
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }
    }
}
