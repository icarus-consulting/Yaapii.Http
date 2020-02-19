using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Body.Test
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
