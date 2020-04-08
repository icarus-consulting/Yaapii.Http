using Xunit;
using Yaapii.Http.Requests;
using Yaapii.Xml;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class XmlBodyOfTests
    {
        [Fact]
        public void ReadsXml()
        {
            var expected = new XMLCursor("<expected />");
            Assert.Equal(
                expected.AsNode().ToString(),
                new XmlBody.Of(
                    new Request(
                        new Body(expected)
                    )
                ).AsNode().ToString()
            );
        }
    }
}
