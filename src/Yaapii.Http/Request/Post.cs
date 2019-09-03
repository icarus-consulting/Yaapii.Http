using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Http.Request
{
    public sealed class Post : DictEnvelope
    {
        public Post(params IDictInput[] inputs) : this(new EnumerableOf<IDictInput>(inputs))
        { }

        public Post(IEnumerable<IDictInput> inputs) : this("", inputs)
        { }

        public Post(string uri, params IDictInput[] inputs) : this(
            uri,
            new EnumerableOf<IDictInput>(inputs)
        )
        { }

        public Post(string uri, IEnumerable<IDictInput> inputs) : base(
            () => new Basic(uri, "POST", inputs)
        )
        { }
    }
}
