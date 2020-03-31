﻿//MIT License

//Copyright(c) 2020 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Yaapii.Atoms.Enumerable;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds header fields to a request.
    /// The same key can be used multiple times to add multiple values to the same header field.
    /// </summary>
    public sealed partial class Headers : MapInput.Envelope
    {
        private const string KEY_PREFIX = "header:";
        private const string INDEX_SEPARATOR = ":";

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(params string[] pairSequence) : this(new Many.Of(pairSequence))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(IEnumerable<string> pairSequence) : this(
            new Many.Of<IKvp>(() =>
            {
                var length = new Atoms.Enumerable.LengthOf(pairSequence).Value();
                if (length % 2 != 0)
                {
                    throw 
                        new ArgumentException(
                            "Can not create headers from pair sequence. " +
                            $"The number of given strings needs to be a multiple of two, but is {length}."
                        );
                }
                var result = new List<IKvp>();
                for(int i = 0; i < length; i += 2)
                {
                    result.Add(
                        new Kvp.Of(
                            new ItemAt<string>(pairSequence, i).Value(),
                            new ItemAt<string>(pairSequence, i+1).Value()
                        )
                    );
                }
                return result;
            })
        )
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(params IKvp[] headers) : this(new Many.Of<IKvp>(headers))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(params KeyValuePair<string, string>[] headers) : this(new Many.Of<KeyValuePair<string, string>>(headers))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(NameValueCollection headers) : this(
            new Many.Of<IKvp>(() =>
            {
                var kvps = new List<IKvp>();
                foreach(var key in headers.AllKeys)
                {
                    foreach(var value in headers.GetValues(key))
                    {
                        kvps.Add(
                            new Kvp.Of(key, value)
                        );
                    }
                }
                return kvps;
            })
        )
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(IEnumerable<KeyValuePair<string, string>> headers) : this(
            new Atoms.Enumerable.Mapped<KeyValuePair<string, string>, IKvp>(kvp =>
                new Kvp.Of(kvp.Key, kvp.Value),
                headers
            )
        )
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(IEnumerable<IKvp> headers) : base(input =>
        {
            int index =
                new Atoms.Enumerable.LengthOf(
                    new Headers.Of(input)
                ).Value();
            return
                new MapInput.Of(
                    new Atoms.Enumerable.Mapped<IKvp, IKvp>(kvp =>
                        new Kvp.Of(
                            $"{KEY_PREFIX}{index++}{INDEX_SEPARATOR}{kvp.Key()}", 
                            kvp.Value()
                        ),
                        headers
                    )
                ).Apply(input);
        })
        { }
    }
}
