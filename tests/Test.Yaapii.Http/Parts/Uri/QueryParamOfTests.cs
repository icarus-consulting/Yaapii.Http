using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class QueryParamOfTests
    {
        [Fact]
        public void ReadsParam()
        {
            Assert.Equal(
                "some value",
                new QueryParam.Of(
                    new Map.Of("query:some key", "some value"),
                    "some key"
                ).AsString()
            );
        }
    }
}
