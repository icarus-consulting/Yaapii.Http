using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Test
{
    public sealed class MethodOfTests
    {
        [Fact]
        public void ReadsMethod()
        {
            Assert.Equal(
                "post",
                new Method.Of(
                    new Map.Of("method", "post")
                ).AsString()
            );
        }
    }
}
