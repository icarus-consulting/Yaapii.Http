using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http query parameters.
    /// </summary>
    public sealed class QueryParams : DictInputEnvelope
    {
        /// <summary>
        /// Http query parameters.
        /// </summary>
        public QueryParams(params QueryParam[] queryParams)
        { }

        /// <summary>
        /// Http query parameters.
        /// </summary>
        public QueryParams(IEnumerable<QueryParam> queryParams) : base(
            dict => 
            new DictOf(
                new Mapped<QueryParam, IDictInput>(
                    q => q.Self(),
                    queryParams
                )
            )
        )
        { }

        /// <summary>
        /// Query parameters from response.
        /// </summary>
        public sealed class Of : IText
        {
            private readonly IDict response;

            public Of(IDict response)
            {
                this.response = response;
            }

            public string AsString()
            {
                return this.QueryString();
            }

            public bool Equals(IText other)
            {
                return this.AsString().Equals(other.AsString());
            }

            private string QueryString()
            {
                var parts = 
                    new Mapped<IKvp, IKvp>(
                        kvp => new KvpOf(kvp.Key().Substring(2), kvp.Value()),
                        new Filtered<IKvp>(
                            kvp => kvp.Key().StartsWith("q."),
                            this.response.Entries()
                        )
                    );

                NameValueCollection query = new NameValueCollection();
                foreach(var part in parts)
                {
                    query[part.Key()] = part.Value();
                }
                var builder = new UriBuilder();
                builder.Query = query.ToString();
                return builder.Query.ToString();
            }
        }
    }
}
