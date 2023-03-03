//MIT License

//Copyright(c) 2023 ICARUS Consulting GmbH

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
using Yaapii.Atoms;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds header fields to a request.
    /// The same key can be used multiple times to add multiple values to the same header field.
    /// </summary>
    public sealed partial class Headers : MessageInputEnvelope
    {
        private const string KEY_PREFIX = "header:";
        private const string INDEX_SEPARATOR = ":";

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(params string[] pairSequence) : this(new ManyOf(pairSequence))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(IEnumerable<string> pairSequence) : this(
            new ManyOf<IKvp>(() =>
            {
                var length = new LengthOf(pairSequence).Value();
                if (length % 2 != 0)
                {
                    throw
                        new ArgumentException(
                            "Can not create headers from pair sequence. " +
                            $"The number of given strings needs to be a multiple of two, but is {length}."
                        );
                }
                var result = new List<IKvp>();
                for (int i = 0; i < length; i += 2)
                {
                    result.Add(
                        new KvpOf(
                            new ItemAt<string>(pairSequence, i).Value(),
                            new ItemAt<string>(pairSequence, i + 1).Value()
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
        public Headers(params IKvp[] headers) : this(new ManyOf<IKvp>(headers))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(params KeyValuePair<string, string>[] headers) : this(new MapOf(headers))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(NameValueCollection headers) : this(
            new ManyOf<IKvp>(() =>
            {
                var kvps = new List<IKvp>();
                foreach (var key in headers.AllKeys)
                {
                    foreach (var value in headers.GetValues(key))
                    {
                        kvps.Add(
                            new KvpOf(key, value)
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
        public Headers(IDictionary<string, string> headers) : this(
            new Atoms.Enumerable.Mapped<string, IKvp>(key =>
                new KvpOf(key, headers[key]),
                headers.Keys
            )
        )
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(IEnumerable<IKvp> headers) : base((message) =>
        {
            int index =
                new Atoms.Enumerable.LengthOf(
                    new Headers.Of(message)
                ).Value();
            return
                new SimpleMessageInput(
                    new Atoms.Enumerable.Mapped<IKvp, IKvp>(kvp =>
                        new KvpOf(
                            $"{KEY_PREFIX}{index++}{INDEX_SEPARATOR}{kvp.Key()}",
                            kvp.Value()
                        ),
                        headers
                    )
                ).Apply(message);
        })
        { }
    }
}
