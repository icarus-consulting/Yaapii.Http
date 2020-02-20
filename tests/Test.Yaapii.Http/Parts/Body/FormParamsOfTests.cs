using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class FormParamsOfTests
    {
        [Theory]
        [InlineData("first key", "first value")]
        [InlineData("second key", "second value")]
        [InlineData("third key", "third value")]
        public void ReadsParams(string key, string expected)
        {
            Assert.Equal(
                expected,
                new FormParams.Of(
                    new Map.Of(
                        "form:first key", "first value",
                        "form:second key", "second value",
                        "form:third key", "third value"
                    )
                )[key]
            );
        }
    }
}
