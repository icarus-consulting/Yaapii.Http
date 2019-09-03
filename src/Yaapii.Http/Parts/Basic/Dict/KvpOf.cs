using System;
using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// Key-value pair made of strings.
    /// </summary>
    public sealed class KvpOf : IKvp
    {
        private readonly Lazy<KeyValuePair<string, string>> entry;

        public KvpOf(string key, IText value) : this(
            new TextOf(key),
            value
        )
        { }

        public KvpOf(IText key, string value) : this(
            key,
            new TextOf(value)
        )
        { }

        public KvpOf(string key, string value) : this(
            new StickyText(key),
            new StickyText(value)
        )
        { }

        public KvpOf(IText key, IText value) : this(
            new ScalarOf<KeyValuePair<string, string>>(() =>
                 new KeyValuePair<string, string>(key.AsString(), value.AsString())
            )
        )
        { }

        /// <summary>
        /// Key-value pair made of strings.
        /// </summary>
        public KvpOf(IScalar<KeyValuePair<string, string>> kvp)
        {
            this.entry =
                new Lazy<KeyValuePair<string, string>>(
                    () => kvp.Value()
                );
        }

        public string Key()
        {
            return this.entry.Value.Key;
        }

        public string Value()
        {
            return this.entry.Value.Value;
        }
    }

    /// <summary>
    /// Key-value pair matching a string to specified type value.
    /// </summary>
    public sealed class KvpOf<TValue> : IKvp<TValue>
    {
        private readonly Lazy<KeyValuePair<string, Func<TValue>>> entry;
        private readonly Lazy<TValue> value;

        /// <summary>
        /// Key-value pair matching a string to specified type value.
        /// </summary>
        public KvpOf(string key, TValue value) : this(
            new StickyText(key),
            value
        )
        { }

        /// <summary>
        /// Key-value pair matching a string to specified type value.
        /// </summary>
        public KvpOf(string key, Func<TValue> value) : this(
            () => new KeyValuePair<string, Func<TValue>>(key, value)
        )
        { }

        /// <summary>
        /// Key-value pair matching a string to specified type value.
        /// </summary>
        public KvpOf(IText key, Func<TValue> value) : this(
            () => new KeyValuePair<string, Func<TValue>>(
                key.AsString(), value
            )
        )
        { }

        /// <summary>
        /// Key-value pair matching a string to specified type value.
        /// </summary>
        public KvpOf(IText key, TValue value) : this(
            () => new KeyValuePair<string, Func<TValue>>(
                key.AsString(),
                () => value
            )
        )
        { }

        /// <summary>
        /// Key-value pair matching a string to specified type value.
        /// </summary>
        public KvpOf(IScalar<KeyValuePair<string, TValue>> kvp)
        { }

        private KvpOf(Func<KeyValuePair<string, Func<TValue>>> kvp)
        {
            this.entry =
                new Lazy<KeyValuePair<string, Func<TValue>>>(
                    () => kvp.Invoke()
                );
            this.value = new Lazy<TValue>(() => this.entry.Value.Value.Invoke());
        }

        public string Key()
        {
            return this.entry.Value.Key;
        }

        public TValue Value()
        {
            return this.value.Value;
        }
    }
}
