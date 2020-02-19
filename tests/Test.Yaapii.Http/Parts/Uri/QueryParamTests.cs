using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class QueryParamTests
    {
        [Fact]
        public void WritesParam()
        {
            Assert.Equal(
                "some value",
                new QueryParam("some key", "some value").Apply(
                    new Map.Of(new MapInput.Of())
                )["query:some key"]
            );
        }
    }
}
