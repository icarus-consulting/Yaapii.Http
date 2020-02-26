using System.Collections.Generic;
using Yaapii.Atoms.Scalar;
using Yaapii.Xml;
using Yaapii.XML;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class XmlBody
    {
        /// <summary>
        /// Gets the body of a request or response as <see cref="IXML"/>.
        /// </summary>
        public sealed class Of : XMLEnvelope
        {
            /// <summary>
            /// Gets the body of a request or response as <see cref="IXML"/>.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(
                new Sticky<IXML>(() =>
                    new XMLCursor(
                        input["body"]
                    )
                )
            )
            { }
        }
    }
}
