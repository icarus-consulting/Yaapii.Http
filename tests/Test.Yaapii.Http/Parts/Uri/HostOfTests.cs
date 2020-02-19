using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class HostOfTests
    {
        [Fact]
        public void ReadsHost()
        {
            Assert.Equal(
                "localhost",
                new Host.Of(
                    new Map.Of("host", "localhost")
                ).AsString()
            );
        }
    }
}
