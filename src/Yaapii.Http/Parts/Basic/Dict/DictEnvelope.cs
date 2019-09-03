using System;
using System.Collections.Generic;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// Simplified dict building.
    /// </summary>
    public abstract class DictEnvelope : IDict
    {
        private readonly Lazy<IDict> origin;

        /// <summary>
        /// Simplified dict building.
        /// </summary>
        public DictEnvelope(IDict origin) : this(() => origin)
        { }

        /// <summary>
        /// Simplified dict building.
        /// </summary>
        public DictEnvelope(Func<IDict> origin)
        {
            this.origin = new Lazy<IDict>(origin);
        }

        public bool Contains(string key)
        {
            return this.origin.Value.Contains(key);
        }

        public string Content(string key)
        {
            return this.origin.Value.Content(key);
        }

        public string Content(string key, string def)
        {
            return this.origin.Value.Content(key, def);
        }

        public IEnumerable<IKvp> Entries()
        {
            return this.origin.Value.Entries();
        }
    }

    /// <summary>
    /// Simplified dict building.
    /// </summary>
    public abstract class DictEnvelope<TValue> : IDict<TValue>
    {
        private readonly Lazy<IDict<TValue>> origin;

        /// <summary>
        /// Simplified dict building.
        /// </summary>
        public DictEnvelope(IDict<TValue> origin) : this(() => origin)
        { }

        /// <summary>
        /// Simplified dict building.
        /// </summary>
        public DictEnvelope(Func<IDict<TValue>> origin)
        {
            this.origin = new Lazy<IDict<TValue>>(origin);
        }

        public bool Contains(string key)
        {
            return this.origin.Value.Contains(key);
        }

        public TValue Content(string key)
        {
            return this.origin.Value.Content(key);
        }

        public TValue Content(string key, TValue def)
        {
            return this.origin.Value.Content(key, def);
        }

        public IEnumerable<IKvp<TValue>> Entries()
        {
            return this.origin.Value.Entries();
        }
    }
}
