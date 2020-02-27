using Xunit;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Scalar;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class HeadersOfTests
    {
        [Theory]
        [InlineData("first key", "first value")]
        [InlineData("second key", "second value")]
        [InlineData("third key", "third value")]
        public void ReadsParams(string key, string expected)
        {
            Assert.Equal(
                expected,
                new FirstOf<IKvp>(kvp =>
                    kvp.Key() == key,
                    new Headers.Of(
                        new Map.Of(
                            "header:0:first key", "first value",
                            "header:1:second key", "second value",
                            "header:2:third key", "third value"
                        )
                    )
                ).Value().Value()
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
                new ItemAt<IKvp>(
                    new Filtered<IKvp>(kvp =>
                        kvp.Key() == "same key",
                        new Headers.Of(
                            new Map.Of(
                                "header:0:same key", "first value",
                                "header:1:same key", "second value",
                                "header:2:same key", "third value"
                            )
                        )
                    ),
                    index
                ).Value().Value()
            );
        }
    }
}
