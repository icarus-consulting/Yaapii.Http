using Xunit;
using Yaapii.Http.AtomsTemp.Scalar;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Requests.Test
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
