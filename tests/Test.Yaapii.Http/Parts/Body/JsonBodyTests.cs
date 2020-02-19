using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.JSON;

namespace Yaapii.Http.Parts.Body.Test
{
    public sealed class JsonBodyTests
    {
        [Fact]
        public void WritesContentType()
        {
            Assert.Equal(
                "application/json",
                new JsonBody(new JSONOf("{}")).Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Content-Type"]
            );
        }

        [Fact]
        public void WritesBody()
        {
            Assert.Equal(
                "{}",
                new JsonBody(new JSONOf("{}")).Apply(
                    new Map.Of(new MapInput.Of())
                )["body"]
            );
        }
    }
}
