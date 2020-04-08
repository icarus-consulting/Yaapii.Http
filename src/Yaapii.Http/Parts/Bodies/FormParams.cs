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
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds form params to a request.
    /// Sets the content type header to application/x-www-form-urlencoded.
    /// </summary>
    public sealed partial class FormParams : MapInput.Envelope
    {
        private const string KEY_PREFIX = "form:";

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(params string[] pairSequence) : this(new Map.Of(pairSequence))
        { }

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(IEnumerable<string> pairSequence) : this(new Map.Of(pairSequence))
        { }

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(params IKvp[] queryParams) : this(new Map.Of(queryParams))
        { }

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(IEnumerable<IKvp> queryParams) : this(new Map.Of(queryParams))
        { }

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(params KeyValuePair<string, string>[] queryParams) : this(new Map.Of(queryParams))
        { }

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(IEnumerable<KeyValuePair<string, string>> queryParams) : this(new Map.Of(queryParams))
        { }

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(IDictionary<string, string> formParams) : base(() =>
            new Joined(
                new ContentType("application/x-www-form-urlencoded"),
                new MapInput.Of(
                    new Mapped<KeyValuePair<string, string>, IKvp>(origin =>
                        new Kvp.Of($"{KEY_PREFIX}{origin.Key}", origin.Value),
                        formParams
                    )
                )
            )
        )
        { }
    }
}
