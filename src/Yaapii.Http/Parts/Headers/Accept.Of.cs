//MIT License

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
using Yaapii.Http.Responses;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Accept
    {
        /// <summary>
        /// Gets the values of the 'Accept' header field from a request.
        /// Specifies media type(s) that is/are acceptable as a response.
        /// </summary>
        public sealed class Of : HeaderOfEnvelope
        {
            /// <summary>
            /// Gets the values of the 'Accept' header field from a request.
            /// Specifies media type(s) that is/are acceptable as a response.
            /// </summary>
            public Of(Task<IDictionary<string, string>> response) : this(new Synced(response))
            { }

            /// <summary>
            /// Gets the values of the 'Accept' header field from a request.
            /// Specifies media type(s) that is/are acceptable as a response.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(input, KEY)
            { }
        }
    }
}
