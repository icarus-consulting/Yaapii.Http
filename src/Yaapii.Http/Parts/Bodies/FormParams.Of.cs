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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaapii.Atoms;
using Yaapii.Atoms.Map;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class FormParams
    {

        /// <summary>
        /// Form params from a request.
        /// </summary>
        public sealed class Of : MapEnvelope
        {
            /// <summary>
            /// Form params from a request.
            /// </summary>
            public Of(IDictionary<string, string> input) : this(() => input)
            { }

            /// <summary>
            /// Form params from a request.
            /// </summary>
            public Of(Task<IDictionary<string, string>> input) : this(() =>
                Task.Run(() => input).Result
            )
            { }

            /// <summary>
            /// Gets the form params from a request.
            /// </summary>
            private Of(Func<IDictionary<string, string>> input) : base(() =>
                {
                    var ipt = input();
                    return
                        new MapOf(
                            new MappedDictionary<IKvp>((key, value) =>
                                new KvpOf(
                                    key.Remove(0, KEY_PREFIX.Length),
                                    value
                                ),
                                new FilteredDictionary((key, value) =>
                                    key.StartsWith(KEY_PREFIX),
                                    ipt
                                )
                            )
                        );
                },
                live: false
            )
            { }
        }
    }
}
