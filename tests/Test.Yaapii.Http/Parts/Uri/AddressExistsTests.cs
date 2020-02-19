using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class AddressExistsTests
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new Address.Exists(
                    new Map.Of(
                        "scheme", "http",
                        "host", "localhost"
                    )
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new Address.Exists(
                    new Map.Of(new MapInput.Of())
                ).Value()
            );
        }
    }
}
