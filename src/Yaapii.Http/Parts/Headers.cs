using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http headers.
    /// </summary>
    public sealed class Headers : DictEnvelope
    {
        /// <summary>
        /// Http headers.
        /// </summary>
        public Headers(params Header[] headers) : this(
            new EnumerableOf<Header>(headers)
        )
        { }

        /// <summary>
        /// Http headers.
        /// </summary>
        public Headers(IEnumerable<Header> headers) : this(
            new Mapped<Header, IDictInput>(
                header => header.Self(),
                headers
            )
        )
        { }

        public Headers(IEnumerable<IDictInput> headers) : base(() =>
            new DictOf(
                headers
            )
        )
        { }

        /// <summary>
        /// Headers from dictionary.
        /// </summary>
        public sealed class Of : EnumerableEnvelope<IKvp>
        {
            public Of(IDict dict) : base(() =>
                new Mapped<IKvp, IKvp>(
                    kvp => new KvpOf(kvp.Key().Substring(2), kvp.Value()),
                    dict.Entries()
                )
            )
            { }
        }

        public sealed class AsMap : MapEnvelope<string, IList<string>>
        {
            public AsMap(Of flatHeaders) : base(
                new Sticky<IDictionary<string,IList<string>>>(() =>
                {
                    var mapped = new Dictionary<string, IList<string>>();
                    foreach (IKvp header in flatHeaders)
                    {
                        if (!mapped.ContainsKey(header.Key()))
                        {
                            mapped[header.Key()] = new List<string>();
                        }
                        mapped[header.Key()].Add(header.Value());
                    }
                    return mapped;
                })
            )
            { }
        }
    }
}
