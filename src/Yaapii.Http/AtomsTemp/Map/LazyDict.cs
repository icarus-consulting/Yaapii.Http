﻿// MIT License
//
// Copyright(c) 2019 ICARUS Consulting GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using Yaapii.Atoms.Fail;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using Yaapii.Http.AtomsTemp.Enumerable;

namespace Yaapii.Http.AtomsTemp.Lookup
{
    /// <summary>
    /// A dictionary whose values are retrieved only when accessing them.
    /// </summary>
    public sealed class LazyDict : IDictionary<string, string>
    {
        private readonly Lazy<IEnumerable<IKvp>> kvps;
        private readonly Lazy<IDictionary<string, Lazy<string>>> map;
        private readonly UnsupportedOperationException rejectReadException = new UnsupportedOperationException("Writing is not supported, it's a read-only map");

        /// <summary>
        /// ctor
        /// </summary>
        public LazyDict(params IKvp[] kvps) : this(new Many.Live<IKvp>(kvps))
        { }

        /// <summary>
        /// ctor
        /// </summary>
        public LazyDict(IEnumerable<IKvp> kvps)
        {
            this.map =
                new Lazy<IDictionary<string, Lazy<string>>>(() =>
                {
                    var dict = new Dictionary<string, Lazy<string>>();
                    foreach (var kvp in kvps)
                    {
                        dict[kvp.Key()] = new Lazy<string>(() => kvp.Value());
                    }
                    return dict;
                });
        }

        /// <summary>
        /// Access a value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] { get { return map.Value[key].Value; } set { throw this.rejectReadException; } }

        /// <summary>
        /// Access all keys
        /// </summary>
        public ICollection<string> Keys => map.Value.Keys;

        /// <summary>
        /// Access all values
        /// </summary>
        public ICollection<string> Values =>
            new ListOf<string>(
                new Enumerable.Mapped<Lazy<string>, string>(
                    v => v.Value,
                    map.Value.Values
                )
            );


        /// <summary>
        /// Count entries
        /// </summary>
        public int Count => map.Value.Count;

        /// <summary>
        /// Yes its readonly
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, string> item)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        public void Clear()
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Test if map contains entry
        /// </summary>
        /// <param name="item">item to check</param>
        /// <returns>true if it contains</returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return this.map.Value.ContainsKey(item.Key) && this.map.Value[item.Key].Value.Equals(item.Value);
        }

        /// <summary>
        /// Test if map contains key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return this.map.Value.ContainsKey(key);
        }

        /// <summary>
        /// Copy this to an array
        /// </summary>
        /// <param name="array">target array</param>
        /// <param name="arrayIndex">index to start</param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            if (arrayIndex > this.map.Value.Count)
            {
                throw
                    new ArgumentOutOfRangeException(
                        new Formatted(
                            "arrayIndex {0} is higher than the item count in the map {1}.",
                            arrayIndex,
                            this.map.Value.Count
                        ).AsString());
            }

            new ListOf<KeyValuePair<string, string>>(this).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return
                new Enumerable.Mapped<KeyValuePair<string, Lazy<string>>, KeyValuePair<string, string>>(
                    kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.Value),
                    this.map.Value
                ).GetEnumerator();
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Tries to get value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">target to store value</param>
        /// <returns>true if success</returns>
        public bool TryGetValue(string key, out string value)
        {
            var result = this.map.Value.ContainsKey(key);
            if (result)
            {
                value = this.map.Value[key].Value;
                result = true;
            }
            else
            {
                value = string.Empty;
            }
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// A dictionary whose values are retrieved only when accessing them.
    /// </summary>
    public sealed class LazyDict<Value> : IDictionary<string, Value>
    {
        private readonly Lazy<IEnumerable<IKvp<Value>>> kvps;
        private readonly Lazy<IDictionary<string, Lazy<Value>>> map;
        private readonly UnsupportedOperationException rejectReadException = new UnsupportedOperationException("Writing is not supported, it's a read-only map");

        /// <summary>
        /// ctor
        /// </summary>
        public LazyDict(params IKvp<Value>[] kvps) : this(new Many.Live<IKvp<Value>>(kvps))
        { }

        /// <summary>
        /// ctor
        /// </summary>
        public LazyDict(IEnumerable<IKvp<Value>> kvps)
        {
            this.map =
                new Lazy<IDictionary<string, Lazy<Value>>>(() =>
                {
                    var dict = new Dictionary<string, Lazy<Value>>();
                    foreach (var kvp in kvps)
                    {
                        dict[kvp.Key()] = new Lazy<Value>(() => kvp.Value());
                    }
                    return dict;
                });
        }

        /// <summary>
        /// Access a value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Value this[string key] { get { return map.Value[key].Value; } set { throw this.rejectReadException; } }

        /// <summary>
        /// Access all keys
        /// </summary>
        public ICollection<string> Keys => map.Value.Keys;

        /// <summary>
        /// Access all values
        /// </summary>
        public ICollection<Value> Values =>
            new ListOf<Value>(
                new Enumerable.Mapped<Lazy<Value>, Value>(
                    v => v.Value,
                    map.Value.Values
                )
            );


        /// <summary>
        /// Count entries
        /// </summary>
        public int Count => map.Value.Count;

        /// <summary>
        /// Yes its readonly
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, Value value)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, Value> item)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        public void Clear()
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Test if map contains entry
        /// </summary>
        /// <param name="item">item to check</param>
        /// <returns>true if it contains</returns>
        public bool Contains(KeyValuePair<string, Value> item)
        {
            return this.map.Value.ContainsKey(item.Key) && this.map.Value[item.Key].Value.Equals(item.Value);
        }

        /// <summary>
        /// Test if map contains key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return this.map.Value.ContainsKey(key);
        }

        /// <summary>
        /// Copy this to an array
        /// </summary>
        /// <param name="array">target array</param>
        /// <param name="arrayIndex">index to start</param>
        public void CopyTo(KeyValuePair<string, Value>[] array, int arrayIndex)
        {
            if (arrayIndex > this.map.Value.Count)
            {
                throw
                    new ArgumentOutOfRangeException(
                        new Formatted(
                            "arrayIndex {0} is higher than the item count in the map {1}.",
                            arrayIndex,
                            this.map.Value.Count
                        ).AsString());
            }

            new ListOf<KeyValuePair<string, Value>>(this).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<string, Value>> GetEnumerator()
        {
            return
                new Enumerable.Mapped<KeyValuePair<string, Lazy<Value>>, KeyValuePair<string, Value>>(
                    kvp => new KeyValuePair<string, Value>(kvp.Key, kvp.Value.Value),
                    this.map.Value
                ).GetEnumerator();
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, Value> item)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Tries to get value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">target to store value</param>
        /// <returns>true if success</returns>
        public bool TryGetValue(string key, out Value value)
        {
            var result = this.map.Value.ContainsKey(key);
            if (result)
            {
                value = this.map.Value[key].Value;
                result = true;
            }
            else
            {
                value = default(Value);
            }
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// A dictionary whose values are retrieved only when accessing them.
    /// </summary>
    public sealed class LazyDict<Key, Value> : IDictionary<Key, Value>
    {
        private readonly Lazy<IDictionary<Key, Lazy<Value>>> map;
        private readonly UnsupportedOperationException rejectReadException = new UnsupportedOperationException("Writing is not supported, it's a read-only map");

        /// <summary>
        /// ctor
        /// </summary>
        public LazyDict(params IKvp<Key, Value>[] kvps) : this(new Many.Live<IKvp<Key, Value>>(kvps))
        { }

        /// <summary>
        /// ctor
        /// </summary>
        public LazyDict(IEnumerable<IKvp<Key, Value>> kvps)
        {
            this.map =
                new Lazy<IDictionary<Key, Lazy<Value>>>(() =>
                {
                    var dict = new Dictionary<Key, Lazy<Value>>();
                    foreach (var kvp in kvps)
                    {
                        dict[kvp.Key()] = new Lazy<Value>(() => kvp.Value());
                    }
                    return dict;
                });
        }

        /// <summary>
        /// Access a value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Value this[Key key] { get { return map.Value[key].Value; } set { throw this.rejectReadException; } }

        /// <summary>
        /// Access all keys
        /// </summary>
        public ICollection<Key> Keys => map.Value.Keys;

        /// <summary>
        /// Access all values
        /// </summary>
        public ICollection<Value> Values =>
            new ListOf<Value>(
                new Enumerable.Mapped<Lazy<Value>, Value>(
                    v => v.Value,
                    map.Value.Values
                )
            );


        /// <summary>
        /// Count entries
        /// </summary>
        public int Count => map.Value.Count;

        /// <summary>
        /// Yes its readonly
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(Key key, Value value)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<Key, Value> item)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        public void Clear()
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Test if map contains entry
        /// </summary>
        /// <param name="item">item to check</param>
        /// <returns>true if it contains</returns>
        public bool Contains(KeyValuePair<Key, Value> item)
        {
            return this.map.Value.ContainsKey(item.Key) && this.map.Value[item.Key].Value.Equals(item.Value);
        }

        /// <summary>
        /// Test if map contains key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(Key key)
        {
            return this.map.Value.ContainsKey(key);
        }

        /// <summary>
        /// Copy this to an array
        /// </summary>
        /// <param name="array">target array</param>
        /// <param name="arrayIndex">index to start</param>
        public void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
        {
            if (arrayIndex > this.map.Value.Count)
            {
                throw
                    new ArgumentOutOfRangeException(
                        new Formatted(
                            "arrayIndex {0} is higher than the item count in the map {1}.",
                            arrayIndex,
                            this.map.Value.Count
                        ).AsString());
            }

            new ListOf<KeyValuePair<Key, Value>>(this).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
        {
            return
                new Enumerable.Mapped<KeyValuePair<Key, Lazy<Value>>, KeyValuePair<Key, Value>>(
                    kvp => new KeyValuePair<Key, Value>(kvp.Key, kvp.Value.Value),
                    this.map.Value
                ).GetEnumerator();
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(Key key)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<Key, Value> item)
        {
            throw this.rejectReadException;
        }

        /// <summary>
        /// Tries to get value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">target to store value</param>
        /// <returns>true if success</returns>
        public bool TryGetValue(Key key, out Value value)
        {
            var result = this.map.Value.ContainsKey(key);
            if (result)
            {
                value = this.map.Value[key].Value;
                result = true;
            }
            else
            {
                value = default(Value);
            }
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}