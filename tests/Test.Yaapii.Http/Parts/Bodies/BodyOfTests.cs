using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class BodyOfTests
    {
        [Fact]
        public void ReadsBody()
        {
            Assert.Equal(
                "| <-- stick figure body",
                new Body.Of(
                    new Map.Of("body", "| <-- stick figure body")
                ).AsString()
            );
        }
    }
}
