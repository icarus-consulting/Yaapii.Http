using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class FragmentExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Fragment.Exists(
                    new Map.Of("fragment", "qwertz")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Fragment.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
