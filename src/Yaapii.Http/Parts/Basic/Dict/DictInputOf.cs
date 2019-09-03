using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// DictInput from key-value pairs.
    /// </summary>
    public sealed class DictInputOf : DictInputEnvelope
    {
        /// <summary>
        /// DictInput from key-value pairs.
        /// </summary>
        public DictInputOf(params IKvp[] kvps) : this(new Many<IKvp>(kvps))
        { }

        /// <summary>
        /// DictInput from key-value pairs.
        /// </summary>
        public DictInputOf(IEnumerable<IKvp> kvps) : base(kvps)
        { }
    }
}
