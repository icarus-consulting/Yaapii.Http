using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Responses.Test
{
    public sealed class StatusExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Status.Exists(
                    new Map.Of("status", "200")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Status.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
