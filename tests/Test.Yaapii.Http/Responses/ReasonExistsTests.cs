using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Responses.Test
{
    public sealed class ReasonExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Reason.Exists(
                    new Map.Of("reason", "because")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Reason.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
