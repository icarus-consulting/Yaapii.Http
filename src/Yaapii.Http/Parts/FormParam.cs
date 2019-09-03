using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http form parameter for Content-Type application/x-www-form-urlencoded.
    /// </summary>
    public sealed class FormParam : DictInputEnvelope
    {
        public FormParam(string name, string value) : base(
            new KvpOf(
                new Key(name),
                value
            )
        )
        { }

        public sealed class Of : IText
        {
            private readonly string name;
            private readonly IDict dict;

            public Of(string key, IDict dict)
            {
                this.name = key;
                this.dict = dict;
            }

            public string AsString()
            {
                var keyString = new Key(this.name).AsString();
                if(this.dict.Contains(keyString))
                {
                    return this.dict.Content(keyString);
                }
                throw new ArgumentException($"Form parameter {this.name} cannot be found in body.");
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }

        private sealed class Key : IText
        {
            private readonly string name;

            public Key(string name)
            {
                this.name = name;
            }

            public string AsString()
            {
                return string.Format("f.{0}", this.name);
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }
    }
}
