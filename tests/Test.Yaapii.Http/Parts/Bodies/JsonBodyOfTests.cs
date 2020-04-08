using Newtonsoft.Json.Linq;
using Xunit;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class JsonBodyOfTests
    {
        [Fact]
        public void ReadsJson()
        {
            var expected = 
                new JObject(
                    new JProperty("key", "value")
                );
            Assert.Equal(
                expected.ToString(),
                new JsonBody.Of(
                    new Request(
                        new Body(expected)
                    )
                ).Token().ToString()
            );
        }
    }
}
