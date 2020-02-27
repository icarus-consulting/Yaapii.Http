using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class PortExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Port.Exists(
                    new Map.Of("port", "1337")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Port.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
