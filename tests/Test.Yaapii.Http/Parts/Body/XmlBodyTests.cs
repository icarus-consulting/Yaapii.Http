using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Xml;

namespace Yaapii.Http.Parts.Body.Test
{
    public sealed class XmlBodyTests
    {
        [Fact(Skip = "current version of Yaapii.Xml is incompatible with current version of Yaapii.Atoms")]
        public void WritesContentType()
        {
            Assert.Equal(
                "application/xml",
                new XmlBody(new XMLCursor("<irrelevant />")).Apply(
                    new Map.Of(new MapInput.Of())
                )["header:Content-Type"]
            );
        }

        [Fact(Skip = "current version of Yaapii.Xml is incompatible with current version of Yaapii.Atoms")]
        public void WritesBody()
        {
            Assert.Equal(
                "<importantData />",
                new XmlBody(new XMLCursor("<importantData />")).Apply(
                    new Map.Of(new MapInput.Of())
                )["body"]
            );
        }
    }
}
