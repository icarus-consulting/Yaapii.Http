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
using System.Threading.Tasks;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.Number;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Responses
{
    public sealed partial class Status
    {
        /// <summary>
        /// The status code of a response.
        /// </summary>
        public sealed class Of : NumberEnvelope
        {
            /// <summary>
            /// The status code of a response.
            /// </summary>
            public Of(IDictionary<string, string> input) : this(() => input)
            { }

            /// <summary>
            /// The status code of a response.
            /// </summary>
            public Of(Task<IDictionary<string, string>> input) : this(() =>
                Task.Run(() => input).Result
            )
            { }

            /// <summary>
            /// The status code of a response.
            /// </summary>
            private Of(Func<IDictionary<string, string>> input) : base(
                new ScalarOf<int>(() =>
                {
                    var inputValue = input();
                    new FailWhen(
                        !inputValue.ContainsKey(KEY),
                        new InvalidOperationException($"Failed to extract {KEY} from response. No {KEY} found.")
                    ).Go();

                    return
                        new IntOf(
                            new TextOf(() => inputValue[KEY])
                        ).Value();
                })
            )
            { }
        }
    }
}
