using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class PathExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Path.Exists(
                    new Map.Of("path", "/this/is/a/path")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Path.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
