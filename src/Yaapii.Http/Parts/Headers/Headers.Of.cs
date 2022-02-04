﻿//MIT License

//Copyright(c) 2021 ICARUS Consulting GmbH

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
using System.Threading.Tasks;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Responses;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Headers
    {
        /// <summary>
        /// Extracts header values from a request or response.
        /// The same key can occur multiple times, if a header field had multiple values.
        /// </summary>
        public sealed class Of : ManyEnvelope<IKvp>
        {
            /// <summary>
            /// Extracts header values from a request or response.
            /// The same key can occur multiple times, if a header field had multiple values.
            /// </summary>
            public Of(Task<IDictionary<string, string>> response) : this(new Responses.Synced(response))
            { }

            /// <summary>
            /// Extracts header values from a request or response.
            /// The same key can occur multiple times, if a header field had multiple values.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(
                new ScalarOf<IEnumerable<IKvp>>(() =>
                {
                    var headers = new List<IKvp>();
                    foreach (var key in input.Keys)
                    {
                        if (key.StartsWith(KEY_PREFIX))
                        {
                            headers.Add(
                                new KvpOf(
                                    key
                                        .Remove(0, KEY_PREFIX.Length)
                                        .TrimStart('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')
                                        .Remove(0, INDEX_SEPARATOR.Length),
                                    input[key]
                                )
                            );
                        }
                    }
                    return headers;
                }),
                live: false
            )
            { }
        }
    }
}
