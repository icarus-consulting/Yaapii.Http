using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class HtmlBodyTests
    {
        [Fact]
        public void WritesContentType()
        {
            Assert.Equal(
                "text/html",
                new HtmlBody("irrelevant").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Content-Type"]
            );
        }

        [Fact]
        public void WritesBody()
        {
            Assert.Equal(
                "<html />",
                new HtmlBody("<html />").Apply(
                    new Map.Of(new MapInput.Of())
                )["body"]
            );
        }
    }
}
