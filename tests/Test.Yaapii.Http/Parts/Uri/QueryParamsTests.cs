using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class QueryParamsTests
    {
        [Theory]
        [InlineData("first key", "first value")]
        [InlineData("second key", "second value")]
        [InlineData("third key", "third value")]
        public void WritesParams(string key, string expected)
        {
            Assert.Equal(
                expected,
                new QueryParams(
                    "first key", "first value",
                    "second key", "second value",
                    "third key", "third value"
                ).Apply(
                    new Map.Of(new MapInput.Of())
                )[$"query:{key}"]
            );
        }
    }
}
