using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class JsonBodyOfTests
    {
        [Fact]
        public void ReadsBody()
        {
            Assert.Equal(
                "{\r\n  \"key\": \"value\"\r\n}",
                new JsonBody.Of(
                    new Map.Of("body", "{ \"key\" : \"value\" }")
                ).Token().ToString()
            );
        }
    }
}
