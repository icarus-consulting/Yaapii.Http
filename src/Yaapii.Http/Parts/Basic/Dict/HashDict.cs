using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Func;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// A dict implemented using C# Dictionary.
    /// </summary>
    public sealed class HashDict : IDict
    {
        private readonly Lazy<IEnumerable<IKvp>> kvps;
        private readonly Lazy<IDictionary<string, string>> map;

        public HashDict(params IKvp[] kvps) : this(new List<IKvp>(kvps))
        { }

        public HashDict(IEnumerable<IKvp> kvps)
        {
            this.kvps = new Lazy<IEnumerable<IKvp>>(() => new List<IKvp>(kvps));
            this.map =
                new Lazy<IDictionary<string, string>>(() =>
                {
                    var dict = new Dictionary<string, string>();
                    foreach (var kvp in kvps)
                    {
                        dict[kvp.Key()] = kvp.Value();
                    }
                    return dict;
                });
        }

        public bool Contains(string key)
        {
            return this.map.Value.ContainsKey(key);
        }

        public string Content(string key)
        {
            try
            {
                return this.map.Value[key];
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException($"Cannot deliver content for key '{key}' - it is not present in the map.");
            }
        }

        public string Content(string key, string def)
        {
            var result = def;
            if (this.map.Value.ContainsKey(key))
            {
                result = this.map.Value[key];
            }
            return result;
        }

        public IEnumerable<IKvp> Entries()
        {
            return this.kvps.Value;
        }
    }

    /// <summary>
    /// A dict implemented using C# Dictionary.
    /// </summary>
    public sealed class HashDict<TValue> : IDict<TValue>
    {
        private readonly Lazy<IEnumerable<IKvp<TValue>>> kvps;
        private readonly Lazy<IDictionary<string, TValue>> map;

        public HashDict(params IKvp<TValue>[] kvps) : this(new List<IKvp<TValue>>(kvps))
        { }

        public HashDict(IEnumerable<IKvp<TValue>> kvps)
        {
            this.kvps = new Lazy<IEnumerable<IKvp<TValue>>>(() => new List<IKvp<TValue>>(kvps));
            this.map =
                new Lazy<IDictionary<string, TValue>>(() =>
                {
                    var dict = new Dictionary<string, TValue>();
                    foreach (var kvp in kvps)
                    {
                        dict[kvp.Key()] = kvp.Value();
                    }
                    return dict;
                });
        }

        public bool Contains(string key)
        {
            return this.map.Value.ContainsKey(key);
        }

        public TValue Content(string key)
        {
            try
            {
                return this.map.Value[key];
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException($"Cannot deliver content for key '{key}' - it is not present in the map.");
            }
        }

        public TValue Content(string key, TValue def)
        {
            var result = def;
            if (this.map.Value.ContainsKey(key))
            {
                result = this.map.Value[key];
            }
            return result;
        }

        public IEnumerable<IKvp<TValue>> Entries()
        {
            return this.kvps.Value;
        }
    }
}
