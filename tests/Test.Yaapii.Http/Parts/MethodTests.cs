using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Test
{
    public sealed class MethodTests
    {
        [Fact]
        public void WritesMethod()
        {
            Assert.Equal(
                "get",
                new Method("get").Apply(
                    new Map.Of(new MapInput.Of())
                )["method"]
            );
        }
    }
}
