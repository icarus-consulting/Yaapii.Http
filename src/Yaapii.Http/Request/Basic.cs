using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;
using Yaapii.Http.Parts;

namespace Yaapii.Http.Request
{
    /// <summary>
    /// A basic request from uri and method - and optional config.
    /// </summary>
    public sealed class Basic : DictEnvelope
    {
        /// <summary>
        /// A basic request from uri and method - and optional config.
        /// </summary>
        public Basic(string uri, string method, params IDictInput[] inputs) : this(
            uri,
            method,
            new EnumerableOf<IDictInput>(inputs)
        )
        { }

        /// <summary>
        /// A basic request from uri and method - and optional config.
        /// </summary>
        public Basic(string uri, string method, IEnumerable<IDictInput> inputs) : base(
            () =>
            new DictOf(
                new Joined<IDictInput>(
                    new EnumerableOf<IDictInput>(
                        new Path(uri),
                        new Method(method)
                    )
                )
            )
        )
        { }
    }
}
