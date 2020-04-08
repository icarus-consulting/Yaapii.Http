using System.Collections.Generic;
using Yaapii.Atoms.Scalar;
using Yaapii.Xml;
using Yaapii.XML;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// To add an xml body to a request, use new <see cref="Body"/>(<see cref="IXML"/> xml)
    /// </summary>
    public sealed class XmlBody
    {
        /// <summary>
        /// The body of a request or response as <see cref="IXML"/>
        /// </summary>
        public sealed class Of : XMLEnvelope
        {
            /// <summary>
            /// The body of a request or response as <see cref="IXML"/>
            /// </summary>
            public Of(IDictionary<string, string> input) : base(
                new Sticky<IXML>(() =>
                    new XMLCursor(
                        new Body.Of(input)
                    )
                )
            )
            { }
        }
    }
}
