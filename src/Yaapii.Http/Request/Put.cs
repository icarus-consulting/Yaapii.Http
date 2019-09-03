using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Http.Request
{
    public sealed class Put : DictEnvelope
    {
        public Put(params IDictInput[] inputs) : this(new EnumerableOf<IDictInput>(inputs))
        { }

        public Put(IEnumerable<IDictInput> inputs) : this("", inputs)
        { }

        public Put(string uri, params IDictInput[] inputs) : this(
            uri,
            new EnumerableOf<IDictInput>(inputs)
        )
        { }

        public Put(string uri, IEnumerable<IDictInput> inputs) : base(
            () => new Basic(uri, "PUT", inputs)
        )
        { }
    }
}
