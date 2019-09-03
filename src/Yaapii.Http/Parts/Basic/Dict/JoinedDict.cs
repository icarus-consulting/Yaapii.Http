using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// Joined dictionary.
    /// </summary>
    public sealed class JoinedDict : DictEnvelope
    {
        /// <summary>
        /// Joined dictionary.
        /// </summary>
        public JoinedDict(IKvp kvp, IDict origin) : this(
            new HashDict(kvp), origin
        )
        { }

        /// <summary>
        /// Joined dictionary.
        /// </summary>
        public JoinedDict(IDictInput input, IDict origin) : this(
            new DictOf(input), origin
        )
        { }

        /// <summary>
        /// Joined dictionary.
        /// </summary>
        public JoinedDict(params IDict[] dicts) : this(
            new EnumerableOf<IDict>(dicts)
        )
        { }

        /// <summary>
        /// Joined dictionary.
        /// </summary>
        public JoinedDict(IEnumerable<IDict> dicts) : base(
            () =>
                new HashDict(
                    new Joined<IKvp>(
                        new Mapped<IDict, IEnumerable<IKvp>>(
                            dict => dict.Entries(),
                            dicts
                        )
                    )
                )
        )
        { }
    }

    /// <summary>
    /// Joined dictionary.
    /// </summary>
    public sealed class JoinedDict<TValue> : DictEnvelope<TValue>
    {
        /// <summary>
        /// Joined dictionary.
        /// </summary>
        public JoinedDict(IKvp<TValue> kvp, IDict<TValue> origin) : this(
            new HashDict<TValue>(kvp), origin
        )
        { }

        /// <summary>
        /// Joined dictionary.
        /// </summary>
        public JoinedDict(IDictInput<TValue> input, IDict<TValue> origin) : this(
            new DictOf<TValue>(input), origin
        )
        { }

        /// <summary>
        /// Joined dictionary.
        /// </summary>
        public JoinedDict(params IDict<TValue>[] dicts) : this(
            new EnumerableOf<IDict<TValue>>(dicts)
        )
        { }

        /// <summary>
        /// Joined dictionary.
        /// </summary>
        public JoinedDict(IEnumerable<IDict<TValue>> dicts) : base(
            () =>
                new HashDict<TValue>(
                    new Joined<IKvp<TValue>>(
                        new Mapped<IDict<TValue>, IEnumerable<IKvp<TValue>>>(
                            dict => dict.Entries(),
                            dicts
                        )
                    )
                )
        )
        { }
    }
}
