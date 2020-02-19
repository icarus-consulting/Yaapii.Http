using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class PortTests
    {
        [Fact]
        public void WritesPort()
        {
            Assert.Equal(
                "1337",
                new Port(1337).Apply(
                    new Map.Of(new MapInput.Of())
                )["port"]
            );
        }
    }
}
