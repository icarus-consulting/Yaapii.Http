using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Response.Test
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
