using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http header.
    /// </summary>
    public sealed class Header : DictInputEnvelope
    {
        /// <summary>
        /// Http header.
        /// </summary>
        public Header(string key, string value) : this(key, new TextOf(value))
        { }

        /// <summary>
        /// Http header.
        /// </summary>
        public Header(string key, IText value) : base(
            new KvpOf(new Key(key), value)
        )
        { }

        /// <summary>
        /// Header from Dict (Request or Response)
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
                if (this.dict.Contains(dictKey))
                {
                    return this.dict.Content(dictKey);
                }
                throw new ArgumentException($"Header '{this.key}' does not exist.");
            }

            public bool Equals(IText other)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Header Key in Dict.
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
                return $"h.{this.name}";
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }

            private string Normalized(string input)
            {
                char[] chars = input.ToCharArray();
                chars[0] = Upper(chars[0]);
                for (int pos = 1; pos < chars.Length; ++pos)
                {
                    if (chars[pos - 1] == '-')
                    {
                        chars[pos] = Upper(chars[pos]);
                    }
                }
                return new string(chars);
            }

            private char Upper(char c)
            {
                char result = c;
                if (c >= 'a' && c <= 'z')
                {
                    result = (char)(c - ('a' - 'A'));
                }
                return result;
            }
        }
    }
}
