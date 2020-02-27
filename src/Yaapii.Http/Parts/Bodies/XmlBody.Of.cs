using System.Collections.Generic;
using System.Xml.Linq;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Scalar;
using Yaapii.Xml;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class XmlBody
    {
        /// <summary>
        /// Gets the body of a request or response as <see cref="IXML"/>.
        /// </summary>
        public sealed class Of : IXML
        {
            private readonly IScalar<IXML> xml;

            /// <summary>
            /// Gets the body of a request or response as <see cref="IXML"/>.
            /// </summary>
            public Of(IDictionary<string, string> input)
            {
                this.xml =
                    new Sticky<IXML>(() =>
                        new XMLCursor(
                            input["body"]
                        )
                    );
            }

            public XNode AsNode()
            {
                return this.xml.Value().AsNode();
            }

            public IList<IXML> Nodes(string query)
            {
                return this.xml.Value().Nodes(query);
            }

            public IList<string> Values(string query)
            {
                return this.xml.Value().Values(query);
            }

            public IXML WithNamespace(string prefix, object uri)
            {
                return this.xml.Value().WithNamespace(prefix, uri);
            }
        }
    }
}
