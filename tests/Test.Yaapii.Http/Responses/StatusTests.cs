using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Responses.Test
{
    public sealed class StatusTests
    {
        [Fact]
        public void WritesReason()
        {
            Assert.Equal(
                "200",
                new Status(200).Apply(
                    new Map.Of(new MapInput.Of())
                )["status"]
            );
        }
    }
}
