using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Http.Request
{
    public sealed class Delete : DictEnvelope
    {
        public Delete(params IDictInput[] inputs) : this(new EnumerableOf<IDictInput>(inputs))
        { }

        public Delete(IEnumerable<IDictInput> inputs) : this("", inputs)
        { }

        public Delete(string uri, params IDictInput[] inputs) : this(
            uri, 
            new EnumerableOf<IDictInput>(inputs)
        )
        { }

        public Delete(string uri, IEnumerable<IDictInput> inputs) : base(
            () => new Basic(uri, "DELETE", inputs)
        )
        { }
    }
}
