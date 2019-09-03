using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// Dictionary made of dict inputs.
    /// </summary>
    public sealed class DictOf : DictEnvelope
    {
        /// <summary>
        /// Dictionary made of dict inputs.
        /// </summary>
        public DictOf(params IDictInput[] inputs) : this(
            new EnumerableOf<IDictInput>(inputs)
        )
        { }

        /// <summary>
        /// Dictionary made of dict inputs.
        /// </summary>
        public DictOf(IEnumerable<IDictInput> inputs) : base(
            () =>
            {
                IDict dict = new HashDict();
                foreach (IDictInput input in inputs)
                {
                    dict = input.Apply(dict);
                }
                return dict;
            }
        )
        { }
    }

    /// Dictionary made of dict inputs.
    /// </summary>
    public sealed class DictOf<TValue> : DictEnvelope<TValue>
    {
        /// <summary>
        /// Dictionary made of dict inputs.
        /// </summary>
        public DictOf(params IDictInput<TValue>[] inputs) : this(
            new EnumerableOf<IDictInput<TValue>>(inputs)
        )
        { }

        /// <summary>
        /// Dictionary made of dict inputs.
        /// </summary>
        public DictOf(IEnumerable<IDictInput<TValue>> inputs) : base(
            () =>
            {
                IDict<TValue> dict = new HashDict<TValue>();
                foreach (IDictInput<TValue> input in inputs)
                {
                    dict = input.Apply(dict);
                }
                return dict;
            }
        )
        { }
    }
}