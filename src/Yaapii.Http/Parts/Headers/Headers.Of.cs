using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Headers
    {
        /// <summary>
        /// Extracts header values from a request or response.
        /// The same key can occur multiple times, if a header field had multiple values.
        /// </summary>
        public sealed class Of : Many.Envelope<IKvp>
        {
            /// <summary>
            /// Extracts header values from a request or response.
            /// The same key can occur multiple times, if a header field had multiple values.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new Mapped<KeyValuePair<string, string>, IKvp>(kvp =>
                    new Kvp.Of(
                        kvp.Key
                            .Remove(0, KEY_PREFIX.Length)
                            .TrimStart('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')
                            .Remove(0, INDEX_SEPARATOR.Length),
                        kvp.Value
                    ),
                    new Filtered<KeyValuePair<string, string>>(kvp =>
                        kvp.Key.StartsWith(KEY_PREFIX),
                        input
                    )
                )
            )
            { }
        }
    }
}
