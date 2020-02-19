using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Test
{
    public sealed class MethodExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Method.Exists(
                    new Map.Of("method", "post")
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Method.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
