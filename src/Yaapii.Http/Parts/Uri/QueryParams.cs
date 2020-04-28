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
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds query parameters to a request.
    /// </summary>
    public sealed partial class QueryParams : MapInputEnvelope
    {
        private const string KEY_PREFIX = "query:";

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(string query) : this(
            new MapOf(() =>
            {
                if (query.StartsWith("?"))
                {
                    query = query.Remove(0, 1);
                }
                return
                    new MapOf(
                        query.Split(new char[] { '=', '&' })
                    );
            })
        )
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(params string[] pairSequence) : this(new MapOf(pairSequence))
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(IEnumerable<string> pairSequence) : this(new MapOf(pairSequence))
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(params IKvp[] queryParams) : this(new MapOf(queryParams))
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(IEnumerable<IKvp> queryParams) : this(new MapOf(queryParams))
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(params KeyValuePair<string, string>[] queryParams) : this(new MapOf(queryParams))
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(IEnumerable<KeyValuePair<string, string>> queryParams) : this(new MapOf(queryParams))
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(IDictionary<string, string> queryParams) : base(
            new Mapped<KeyValuePair<string, string>, IKvp>(origin =>
                new KvpOf($"{KEY_PREFIX}{origin.Key}", origin.Value),
                queryParams
            )
        )
        { }
    }
}
