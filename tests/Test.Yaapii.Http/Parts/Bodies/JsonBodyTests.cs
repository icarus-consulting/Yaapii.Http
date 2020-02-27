using Newtonsoft.Json.Linq;
using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class JsonBodyTests
    {
        [Fact]
        public void WritesContentType()
        {
            Assert.Equal(
                "application/json",
                new JsonBody(JObject.Parse("{}")).Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Content-Type"]
            );
        }

        [Fact]
        public void WritesBody()
        {
            Assert.Equal(
                "{}",
                new JsonBody(JObject.Parse("{}")).Apply(
                    new Map.Of(new MapInput.Of())
                )["body"]
            );
        }
    }
}
