using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Response
{
    /// <summary>
    /// Http Status.
    /// </summary>
    public sealed class Status : DictInputEnvelope
    {
        private const string KEY = "status";

        /// <summary>
        /// Http Status.
        /// </summary>
        public Status(int status) : this(new TextOf(status))
        { }

        /// <summary>
        /// Http Status.
        /// </summary>
        public Status(string status) : this(new TextOf(status))
        { }

        /// <summary>
        /// Http Status.
        /// </summary>
        public Status(IText status): base(
            new KvpOf(Status.KEY, status)
        )
        { }

        /// <summary>
        /// Http status from response.
        /// </summary>
        public sealed class Of : IText
        {
            private readonly IDict response;
            /// <summary>
            /// Http status from response.
            /// </summary>

            public Of(IDict response)
            {
                this.response = response;
            }

            public string AsString()
            {
                return this.response.Content(Status.KEY);
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }
    }
}
