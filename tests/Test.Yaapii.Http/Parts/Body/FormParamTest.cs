using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Body.Test
{
    public sealed class FormParamTest
    {
        [Fact]
        public void WritesContentType()
        {
            Assert.Equal(
                "application/x-www-form-urlencoded",
                new FormParam("irrelevant", "irrElephant").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Content-Type"]
            );
        }

        [Fact]
        public void WritesParam()
        {
            Assert.Equal(
                "some value",
                new FormParam("some key", "some value").Apply(
                    new Map.Of(new MapInput.Of())
                )["form:some key"]
            );
        }
    }
}
