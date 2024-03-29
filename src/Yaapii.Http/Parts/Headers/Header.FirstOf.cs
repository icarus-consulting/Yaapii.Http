﻿//MIT License

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
using System.Threading.Tasks;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Header
    {
        /// <summary>
        /// First value of any header field with the given key.
        /// </summary>
        public sealed class FirstOf : TextEnvelope
        {
            /// <summary>
            /// First value of any header field with the given key.
            /// </summary>
            public FirstOf(Task<IMessage> input, string key) : this(
                new Responses.Synced(input),
                key
            )
            { }

            /// <summary>
            /// First value of any header field with the given key.
            /// </summary>
            public FirstOf(IMessage input, string key) : base(() =>
                new FirstOf<string>(
                    new Header.Of(input, key),
                    new InvalidOperationException($"Can not find '{key}' in headers.")
                ).Value(), 
                live: false
            )
            { }
        }
    }
}
