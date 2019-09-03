using Yaapii.Atoms;
using Yaapii.Atoms.Dict;

namespace Yaapii.Http.Parts
{
    public sealed class Path : DictInputEnvelope
    {
        private const string KEY = "path";

        public Path(string path) : base(
            dict => new JoinedDict(
                dict,
                new HashDict(
                    new KvpOf(
                        Path.KEY,
                        $"{dict.Content(Path.KEY, "")}{path}"
                    )
                )
            )
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
                return this.dict.Content(Path.KEY);
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }
        }
    }
}
