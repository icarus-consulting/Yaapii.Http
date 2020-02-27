using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class SchemeTests
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
