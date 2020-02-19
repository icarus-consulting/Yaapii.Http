using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Response.Test
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
