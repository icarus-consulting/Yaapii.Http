using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class BodyExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Body.Exists(
                    new Map.Of("body", "| <-- stick figure body")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Body.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
