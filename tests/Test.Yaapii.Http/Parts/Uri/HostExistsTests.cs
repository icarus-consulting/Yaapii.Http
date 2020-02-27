using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class HostExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Host.Exists(
                    new Map.Of("host", "localhost")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Host.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
