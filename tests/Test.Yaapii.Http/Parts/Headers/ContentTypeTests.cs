using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class ContentTypeTests
    {
        [Fact]
        public void WritesHeader()
        {
            Assert.Equal(
                "application/json",
                new ContentType("application/json").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Content-Type"]
            );
        }
    }
}
