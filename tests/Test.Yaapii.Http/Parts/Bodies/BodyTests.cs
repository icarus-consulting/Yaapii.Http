using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class BodyTests
    {
        [Fact]
        public void WritesBody()
        {
            Assert.Equal(
                "| <-- stick figure body",
                new Body("| <-- stick figure body").Apply(
                    new Map.Of(new MapInput.Of())
                )["body"]
            );
        }
    }
}
