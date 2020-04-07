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

using System.Collections.Generic;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class Body
    {
        /// <summary>
        /// The body of a request or response. Returns a given fallback if no body is present.
        /// </summary>
        public sealed class FallbackOf : TextEnvelope
        {
            /// <summary>
            /// The body of a request or response. Returns an empty string if no body is present.
            /// </summary>
            public FallbackOf(IDictionary<string, string> input) : this(input, "")
            { }

            /// <summary>
            /// The body of a request or response. Returns a given fallback if no body is present.
            /// </summary>
            public FallbackOf(IDictionary<string, string> input, string fallback) : base(() =>
            {
                var body = fallback;
                if (new Body.Exists(input).Value())
                {
                    body = new Body.Of(input).AsString();
                }
                return body;
            })
            { }
        }
    }
}
