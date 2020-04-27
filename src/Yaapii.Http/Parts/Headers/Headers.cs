//MIT License

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

using System.Collections.Generic;
using System.Collections.Specialized;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds header fields to a request.
    /// The same key can be used multiple times to add multiple values to the same header field.
    /// </summary>
    public sealed partial class Headers : MapInputEnvelope
    {
        private const string KEY_PREFIX = "header:";
        private const string INDEX_SEPARATOR = ":";

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
        public Headers(params KeyValuePair<string, string>[] headers) : this(new ManyOf<KeyValuePair<string, string>>(headers))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(NameValueCollection headers) : this(
            new ManyOf<IKvp>(() =>
            {
                var kvps = new List<IKvp>();
                foreach(var key in headers.AllKeys)
                {
                    foreach(var value in headers.GetValues(key))
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
        public Headers(IEnumerable<KeyValuePair<string, string>> headers) : this(
            new Mapped<KeyValuePair<string, string>, IKvp>(kvp =>
                new KvpOf(kvp.Key, kvp.Value),
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
                new LengthOf(
                    new Headers.Of(input)
                ).Value();
            return
                new MapInputOf(
                    new Mapped<IKvp, IKvp>(kvp =>
                        new KvpOf(
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
