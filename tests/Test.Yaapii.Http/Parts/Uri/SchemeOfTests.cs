using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class SchemeOfTests
    {
        [Fact]
        public void WritesScheme()
        {
            Assert.Equal(
                "http",
                new Scheme("http").Apply(
                    new Map.Of(new MapInput.Of())
                )["scheme"]
            );
        }
    }
}
