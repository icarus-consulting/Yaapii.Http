using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Scalar;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class AcceptOfTests
    {
        [Fact]
        public void ReadsHeader()
        {
            Assert.Equal(
                "application/json",
                new FirstOf<string>(
                    new Accept.Of(
                        new Map.Of("header:0:Accept", "application/json")
                    )
                ).Value()
            );
        }
    }
}
