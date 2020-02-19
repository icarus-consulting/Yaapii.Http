using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds query parameters to a request.
    /// </summary>
    public sealed partial class QueryParams : MapInput.Envelope
    {
        private const string KEY_PREFIX = "query:";

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(string query) : this(
            new Map.Of(() =>
            {
                if (query.StartsWith("?"))
                {
                    query = query.Remove(0, 1);
                }
                return
                    new Map.Of(
                        query.Split(new char[] { '=', '&' })
                    );
            })
        )
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(params string[] pairSequence) : this(new Map.Of(pairSequence))
        { }

        /// <summary>
        /// Adds query parameters to a request.
        /// </summary>
        public QueryParams(IDictionary<string, string> queryParams) : base(
            new Mapped<KeyValuePair<string, string>, IKvp>(origin =>
                new Kvp.Of($"{KEY_PREFIX}{origin.Key}", origin.Value),
                queryParams
            )
        )
        { }
    }
}
