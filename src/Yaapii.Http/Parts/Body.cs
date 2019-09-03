using System;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http Body.
    /// </summary>
    public sealed class Body : DictInputEnvelope
    {
        private const string KEY = "body";

        public Body(string body) : this(new TextOf(body))
        { }

        public Body(Func<string> body) : this(new TextOf(body))
        { }

        public Body(IInput body) : this(
            new TextOf(body, Encoding.UTF8)
        )
        { }

        public Body(IInput body, Encoding encoding) : this(new TextOf(body, encoding))
        { }

        public Body(byte[] body) : this(body, Encoding.UTF8)
        { }

        public Body(byte[] body, Encoding encoding) : this(
            new TextOf(new BytesOf(body), encoding)
        )
        { }

        public Body(IText body) : base(
            new KvpOf(KEY, body)
        )
        { }

        public sealed class Of : IText
        {
            private readonly IDict dict;

            public Of(IDict dict)
            {
                this.dict = dict;
            }

            public string AsString()
            {
                string body = new FormParams.Of(this.dict).AsString();
                if(body.Length == 0)
                {
                    body = this.dict.Content(Body.KEY);
                }
                return body;
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }
    }
}
