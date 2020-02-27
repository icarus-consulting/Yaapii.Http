using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Responses.Test
{
    public sealed class ReasonTests
    {
        [Fact]
        public void WritesReason()
        {
            Assert.Equal(
                "because",
                new Reason("because").Apply(
                    new Map.Of(new MapInput.Of())
                )["reason"]
            );
        }
    }
}
