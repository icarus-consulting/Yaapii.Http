using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class QueryParamExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new QueryParam.Exists(
                    new Map.Of("query:some key", "some value"),
                    "some key"
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new QueryParam.Exists(
                    new Map.Of("query:some key", "some value"),
                    "some nonexistant key"
                ).Value()
            );
        }
    }
}
