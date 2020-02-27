using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Responses.Test
{
    public sealed class ReasonOfTests
    {
        [Fact]
        public void ReadsReason()
        {
            Assert.Equal(
                "because",
                new Reason.Of(
                    new Map.Of("reason", "because")
                ).AsString()
            );
        }
    }
}
