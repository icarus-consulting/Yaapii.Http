using Xunit;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Request.Test
{
    public sealed class RefinedTests
    {
        [Fact]
        public void AddsRequestParts()
        {
            Assert.Equal(
                "some token",
                new FirstOf<string>(
                    new BearerTokenAuth.Of(
                        new Refined(
                            new Get(),
                            new BearerTokenAuth("some token")
                        )
                    )
                ).Value()
            );
        }
    }
}
