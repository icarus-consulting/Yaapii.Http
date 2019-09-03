using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// A key to many strings.
    /// </summary>
    public sealed class KeyToMany : KvpEnvelope<IEnumerable<string>>
    {
        /// <summary>
        /// A key to many strings.
        /// </summary>
        public KeyToMany(string key, params Func<string>[] many) : this(key, () => new Many<string>(many))
        { }

        /// <summary>
        /// A key to many strings.
        /// </summary>
        public KeyToMany(string key, params string[] many) : this(key, () => new Many<string>(many))
        { }

        /// <summary>
        /// A key to many strings.
        /// </summary>
        public KeyToMany(string key, IEnumerable<string> many) : this(key, () => many)
        { }

        /// <summary>
        /// A key to many strings.
        /// </summary>
        public KeyToMany(string key, Func<IEnumerable<string>> many) : base(
            new KvpOf<IEnumerable<string>>(key, many)
        )
        { }
    }

    /// <summary>
    /// A key to many values.
    /// </summary>
    public sealed class KeyToMany<TValue> : KvpEnvelope<IEnumerable<TValue>>
    {
        /// <summary>
        /// A key to many values.
        /// </summary>
        public KeyToMany(string key, IEnumerable<TValue> many) : this(key, () => many)
        { }

        /// <summary>
        /// A key to many values.
        /// </summary>
        public KeyToMany(string key, Func<IEnumerable<TValue>> many) : base(
            new KvpOf<IEnumerable<TValue>>(key, many)
        )
        { }
    }
}
