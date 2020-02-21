using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class FormParamExistsTest
    {
        [Fact]
        public void ReturnsExists()
        {
            Assert.True(
                new FormParam.Exists(
                    new Map.Of("form:some key", "some value"),
                    "some key"
                ).Value()
            );
        }

        [Fact]
        public void ReturnsDoesNotExist()
        {
            Assert.False(
                new FormParam.Exists(
                    new Map.Of("form:some key", "some value"),
                    "nonexistant param key"
                ).Value()
            );
        }
    }
}
