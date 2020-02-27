using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Xml;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class XmlBodyTests
    {
        public void WritesContentType()
        {
            Assert.Equal(
                "application/xml",
                new XmlBody(new XMLCursor("<irrelevant />")).Apply(
                    new Map.Of(new MapInput.Of())
                )["header:Content-Type"]
            );
        }
        
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
