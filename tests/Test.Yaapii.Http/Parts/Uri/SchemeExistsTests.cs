using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class SchemeExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Scheme.Exists(
                    new Map.Of("scheme", "http")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Scheme.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
