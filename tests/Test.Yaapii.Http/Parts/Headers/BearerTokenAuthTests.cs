using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class BearerTokenAuthTests
    {
        [Fact]
        public void WritesHeader()
        {
            Assert.Equal(
                "Bearer this is a token",
                new BearerTokenAuth("this is a token").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Authorization"]
            );
        }
    }
}
