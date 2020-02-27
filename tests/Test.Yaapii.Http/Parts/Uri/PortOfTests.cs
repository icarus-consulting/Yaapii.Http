using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class PortOfTests
    {
        [Fact]
        public void ReadsPort()
        {
            Assert.Equal(
                1337,
                new Port.Of(
                    new Map.Of("port", "1337")
                ).AsInt()
            );
        }
    }
}
