using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Scalar;
using Yaapii.Xml;
using Yaapii.XML;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http Body as XML.
    /// </summary>
    public sealed class XmlBody : DictInputEnvelope
    {
        /// <summary>
        /// Http Body as XML.
        /// </summary>
        public XmlBody(IXML source) : base(() => 
            new Body(() => source.ToString())
        )
        { }

        /// <summary>
        /// Xml Body from Response.
        /// </summary>
        public sealed class Of : XMLEnvelope
        {
            public Of(IDict response) : base(new Sticky<IXML>(() =>
                    new XMLCursor(
                        new Body.Of(response)
                    )
                )
            )
            { }
        }
    }
}
