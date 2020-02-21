using System.Collections.Generic;
using Yaapii.Atoms.Bytes;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class BytesBody
    {
        /// <summary>
        /// Gets the body of a request or response as <see cref="Yaapii.Atoms.IBytes"/>.
        /// Bytes will be decoded from base 64.
        /// </summary>
        public sealed class Of : BytesEnvelope
        {
            /// <summary>
            /// Gets the body of a request or response as <see cref="Yaapii.Atoms.IBytes"/>.
            /// Bytes will be decoded from base 64.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new Base64Bytes(
                    new BytesOf(
                        new Body.Of(input)
                    )
                ).AsBytes()
            )
            { }
        }
    }
}
