using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class AcceptTests
    {
        [Fact]
        public void WritesHeader()
        {
            Assert.Equal(
                "application/json",
                new Accept("application/json").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Accept"]
            );
        }
    }
}
