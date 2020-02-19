using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Response.Test
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
