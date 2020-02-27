using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class QueryParamsOfTests
    {
        [Theory]
        [InlineData("first key", "first value")]
        [InlineData("second key", "second value")]
        [InlineData("third key", "third value")]
        public void ReadsParams(string key, string expected)
        {
            Assert.Equal(
                expected,
                new QueryParams.Of(
                    new Map.Of(
                        "query:first key", "first value",
                        "query:second key", "second value",
                        "query:third key", "third value"
                    )
                )[key]
            );
        }
    }
}
