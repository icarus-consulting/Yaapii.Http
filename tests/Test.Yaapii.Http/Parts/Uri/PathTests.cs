using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class PathTests
    {
        [Fact]
        public void WritesPath()
        {
            Assert.Equal(
                "/this/is/a/path",
                new Path("this/is/a/path").Apply(
                    new Map.Of(new MapInput.Of())
                )["path"]
            );
        }
    }
}
