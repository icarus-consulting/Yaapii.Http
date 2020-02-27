using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
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
