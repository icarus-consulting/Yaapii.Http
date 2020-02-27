using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Responses.Test
{
    public sealed class StatusOfTests
    {
        [Fact]
        public void ReadsReason()
        {
            Assert.Equal(
                200,
                new Status.Of(
                    new Map.Of("status", "200")
                ).AsInt()
            );
        }
    }
}
