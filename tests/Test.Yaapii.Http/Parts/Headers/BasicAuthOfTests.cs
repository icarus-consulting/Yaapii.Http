using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Scalar;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class BasicAuthOfTests
    {
        [Fact]
        public void ReadsHeader()
        {
            Assert.Equal(
                "user:password",
                new FirstOf<string>(
                    new BasicAuth.Of(
                        new Map.Of("header:0:Authorization", $"Basic {new TextBase64("user:password").AsString()}")
                    )
                ).Value()
            );
        }
    }
}
