using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class TextBodyTests
    {
        [Fact]
        public void WritesContentType()
        {
            Assert.Equal(
                "text/plain",
                new TextBody("irrelevant").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Content-Type"]
            );
        }

        [Fact]
        public void WritesBody()
        {
            Assert.Equal(
                "some body",
                new TextBody("some body").Apply(
                    new Map.Of(new MapInput.Of())
                )["body"]
            );
        }
    }
}
