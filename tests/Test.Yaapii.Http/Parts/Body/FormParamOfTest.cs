using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Body.Test
{
    public sealed class FormParamOfTest
    {
        [Fact]
        public void ReadsParam()
        {
            Assert.Equal(
                "some value",
                new FormParam.Of(
                    new Map.Of("form:some key", "some value"),
                    "some key"
                ).AsString()
            );
        }
    }
}
