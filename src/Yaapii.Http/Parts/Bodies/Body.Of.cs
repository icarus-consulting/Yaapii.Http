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
using System.IO;
using System.Threading.Tasks;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class Body
    {
        /// <summary>
        /// Gets the body of a request or response.
        /// The body will be decoded from base 64.
        /// </summary>
        public sealed class Of : IInput
        {
            private readonly IScalar<IInput> input;

            /// <summary>
            /// The body of a request or response.
            /// </summary>
            public Of(IDictionary<string, string> input) : this(() => input)
            { }

            /// <summary>
            /// The body of a request or response.
            /// </summary>
            public Of(Task<IDictionary<string, string>> input) : this(() =>
                input.Result
            )
            { }

            /// <summary>
            /// The body of a request or response.
            /// </summary>
            private Of(Func<IDictionary<string, string>> input) : this(
                new ScalarOf<IInput>(() =>
                {
                    IInput result = new DeadInput();
                    var ipt = input();
                    if(ipt.ContainsKey(Body.KEY))
                    {
                        result =
                            new InputOf(
                                new Base64Bytes(
                                    new BytesOf(
                                        ipt[KEY]
                                    )
                                )
                            );
                    }
                    return result;
                }
                    
                )
            )
            { }

            private Of(IScalar<IInput> input)
            {
                this.input = input;
            }

            /// <summary>
            /// The body of a request or response
            /// </summary>
            /// <returns></returns>
            public Stream Stream()
            {
                return this.input.Value().Stream();
            }
        }
    }
}
