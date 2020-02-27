using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class AuthorizationTests
    {
        [Fact]
        public void WritesHeader()
        {
            Assert.Equal(
                "Basic dXNlcjpwYXNzd29yZA==",
                new Authorization("Basic dXNlcjpwYXNzd29yZA==").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Authorization"]
            );
        }
    }
}
