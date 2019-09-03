using Yaapii.Atoms;
using Yaapii.Atoms.Dict;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http request uri.
    /// </summary>
    public sealed class RequestUri : DictInputEnvelope
    {
        private const string KEY = "uri";

        /// <summary>
        /// Http request uri.
        /// </summary>
        public RequestUri(string uri) : base(
            new KvpOf(KEY, uri)
        )
        { }

        /// <summary>
        /// Http request uri from dict.
        /// </summary>
        public sealed class Of : IText
        {
            private readonly IDict dict;

            /// <summary>
            /// Http request uri from dict.
            /// </summary>
            public Of(IDict dict)
            {
                this.dict = dict;
            }

            public string AsString()
            {
                return string.Format(
                    "{0}{1}{2}",
                    this.dict.Content(RequestUri.KEY, ""),
                    new Path.Of(this.dict).AsString(),
                    new QueryParams.Of(this.dict).AsString()
                );
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }
    }
}
