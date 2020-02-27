using Xunit;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Scalar;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class HeaderOfTests
    {
        [Fact]
        public void ReadsHeader()
        {
            Assert.Equal(
                "some value",
                new FirstOf<string>(
                    new Header.Of(
                        new Map.Of("header:0:some key", "some value"),
                        "some key"
                    )
                ).Value()
            );
        }

        [Theory]
        [InlineData(0, "first value")]
        [InlineData(1, "second value")]
        [InlineData(2, "third value")]
        public void ReadsMultipleValues(int index, string expected)
        {
            Assert.Equal(
                expected,
                new ItemAt<string>(
                    new Header.Of(
                        new Map.Of(
                            "header:0:same key", "first value",
                            "header:1:same key", "second value",
                            "header:2:same key", "third value"
                        ),
                        "same key"
                    ),
                    index
                ).Value()
            );
        }
    }
}
