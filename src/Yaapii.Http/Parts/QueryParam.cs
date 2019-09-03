using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http query parameter.
    /// </summary>
    public sealed class QueryParam : DictInputEnvelope
    {
        /// <summary>
        /// Http query parameter.
        /// </summary>
        public QueryParam(string key, string value) : base(
            new KvpOf(new Key(key), value)
        )
        { }

        /// <summary>
        /// Query parameter from dictionary.
        /// </summary>
        public sealed class Of : IText
        {
            private readonly string key;
            private readonly IDict dict;

            public Of(string key, IDict dict)
            {
                this.key = key;
                this.dict = dict;
            }

            public string AsString()
            {
                var dictKey = new Key(this.key).AsString();
                if(this.dict.Contains(dictKey))
                {
                    return this.dict.Content(dictKey);
                }
                throw new ArgumentException($"Cannot find query parameter {this.key} in url.");
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }

        /// <summary>
        /// Internal key for query parameter.
        /// </summary>
        private sealed class Key : IText
        {
            private readonly string name;

            public Key(string name)
            {
                this.name = name;
            }

            public string AsString()
            {
                return $"q.{this.name}";
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }
    }
}
