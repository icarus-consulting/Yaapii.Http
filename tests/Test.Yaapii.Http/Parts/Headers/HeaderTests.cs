using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class HeaderTests
    {
        [Fact]
        public void WritesHeader()
        {
            Assert.Equal(
                "some value",
                new Header("some key", "some value").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:some key"]
            );
        }

        [Theory]
        [InlineData(0, "first value")]
        [InlineData(1, "second value")]
        [InlineData(2, "third value")]
        public void WritesMultipleValues(int index, string expected)
        {
            Assert.Equal(
                expected,
                new Request(
                    new Header("same key", "first value"),
                    new Header("same key", "second value"),
                    new Header("same key", "third value")
                )[$"header:{index}:same key"]
            );
        }
    }
}
