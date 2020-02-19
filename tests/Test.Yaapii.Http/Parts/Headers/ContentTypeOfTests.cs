using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class ContentTypeOfTests
    {
        [Fact]
        public void ReadsHeader()
        {
            Assert.Equal(
                "application/json",
                new FirstOf<string>(
                    new ContentType.Of(
                        new Map.Of("header:0:Content-Type", $"application/json")
                    )
                ).Value()
            );
        }
    }
}
