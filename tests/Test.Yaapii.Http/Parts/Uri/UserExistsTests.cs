using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class UserExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new User.Exists(
                    new Map.Of("user", "yourNameHere")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new User.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
