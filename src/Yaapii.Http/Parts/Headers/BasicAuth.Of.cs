using System.Collections.Generic;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class BasicAuth
    {
        /// <summary>
        /// Extracts credentials from basic authorization headers.
        /// </summary>
        public sealed class Of : Many.Envelope<string>
        {
            /// <summary>
            /// Extracts credentials from basic authorization headers.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new Mapped<string, string>(basicAuth =>
                    new Base64Text(
                        basicAuth.Remove(0, AUTH_PREFIX.Length)
                    ).AsString(),
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
