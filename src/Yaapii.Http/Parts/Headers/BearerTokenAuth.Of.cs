using System.Collections.Generic;
using Yaapii.Http.AtomsTemp.Enumerable;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class BearerTokenAuth
    {
        /// <summary>
        /// Extracts tokens from bearer token authorization headers.
        /// </summary>
        public sealed class Of : Many.Envelope<string>
        {
            /// <summary>
            /// Extracts tokens from bearer token authorization headers.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new Mapped<string, string>(bearerAuth =>
                    bearerAuth.Remove(0, AUTH_PREFIX.Length),
                    new Filtered<string>(auth =>
                        auth.StartsWith(AUTH_PREFIX),
                        new Authorization.Of(input)
                    )
                )
            )
            { }
        }
    }
}
