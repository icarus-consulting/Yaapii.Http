using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class PathOfTests
    {
        [Fact]
        public void ReadsPath()
        {
            Assert.Equal(
                "/this/is/a/path",
                new Path.Of(
                    new Map.Of("path", "/this/is/a/path")
                ).AsString()
            );
        }
    }
}
