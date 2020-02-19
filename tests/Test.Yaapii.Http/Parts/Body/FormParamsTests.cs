using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Body.Test
{
    public sealed class FormParamsTests
    {
        [Fact]
        public void WritesContentType()
        {
            Assert.Equal(
                "application/x-www-form-urlencoded",
                new FormParams("irrelevant", "irrelevant").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Content-Type"]
            );
        }

        [Theory]
        [InlineData("first key", "first value")]
        [InlineData("second key", "second value")]
        [InlineData("third key", "third value")]
        public void WritesParams(string key, string expected)
        {
            Assert.Equal(
                expected,
                new FormParams(
                    "first key", "first value",
                    "second key", "second value",
                    "third key", "third value"
                ).Apply(
                    new Map.Of(new MapInput.Of())
                )[$"form:{key}"]
            );
        }
    }
}
