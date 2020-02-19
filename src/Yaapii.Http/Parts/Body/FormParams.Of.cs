using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Body
{
    public sealed partial class FormParams : MapInput.Envelope
    {
        /// <summary>
        /// Gets the form params from a request.
        /// </summary>
        public sealed class Of : Map.Envelope
        {
            /// <summary>
            /// Gets the form params from a request.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new Map.Of(
                    new Mapped<KeyValuePair<string, string>, IKvp>(origin =>
                        new Kvp.Of(
                            origin.Key.Remove(0, KEY_PREFIX.Length),
                            origin.Value
                        ),
                        new Filtered<KeyValuePair<string, string>>(kvp =>
                            kvp.Key.StartsWith(KEY_PREFIX),
                            input
                        )
                    )
                )
            )
            { }
        }
    }
}
