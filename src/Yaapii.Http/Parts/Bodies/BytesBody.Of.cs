using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// To add bytes as a body to a request, use new <see cref="Body"/>(<see cref="IBytes"/> content)
    /// </summary>
    public sealed class BytesBody
    {
        /// <summary>
        /// The body of a request or response as <see cref="IBytes"/>
        /// </summary>
        public sealed class Of : BytesEnvelope
        {
            /// <summary>
            /// The body of a request or response as <see cref="IBytes"/>
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new BytesOf(
                    new Body.Of(input)
                ).AsBytes()
            )
            { }
        }
    }
}
