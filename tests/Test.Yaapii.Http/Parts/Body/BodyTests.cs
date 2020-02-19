using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Body.Test
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
