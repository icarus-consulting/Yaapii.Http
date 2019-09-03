using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Http.Request
{
    public sealed class Get : DictEnvelope
    {
        public Get(params IDictInput[] inputs) : this(new EnumerableOf<IDictInput>(inputs))
        { }

        public Get(IEnumerable<IDictInput> inputs) : this("", inputs)
        { }

        public Get(string uri, params IDictInput[] inputs) : this(
            uri,
            new EnumerableOf<IDictInput>(inputs)
        )
        { }

        public Get(string uri, IEnumerable<IDictInput> inputs) : base(
            () => new Basic(uri, "GET", inputs)
        )
        { }
    }
}
