using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class HeadersTests
    {
        [Theory]
        [InlineData(0, "first key", "first value")]
        [InlineData(1, "second key", "second value")]
        [InlineData(2, "third key", "third value")]
        public void WritesParams(int index, string key, string expected)
        {
            Assert.Equal(
                expected,
                new Headers(
                    new Kvp.Of("first key", "first value"),
                    new Kvp.Of("second key", "second value"),
                    new Kvp.Of("third key", "third value")
                ).Apply(
                    new Map.Of(new MapInput.Of())
                )[$"header:{index}:{key}"]
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
                new Headers(
                    new Kvp.Of("same key", "first value"),
                    new Kvp.Of("same key", "second value"),
                    new Kvp.Of("same key", "third value")
                ).Apply(
                    new Map.Of(new MapInput.Of())
                )[$"header:{index}:same key"]
            );
        }
    }
}
