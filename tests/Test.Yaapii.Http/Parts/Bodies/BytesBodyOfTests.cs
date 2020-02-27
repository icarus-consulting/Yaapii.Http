using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class BytesBodyOfTests
    {
        [Fact]
        public void DecodesBase64()
        {
            Assert.Equal(
                "this is a test",
                new TextOf(
                    new BytesBody.Of(
                        new Map.Of("body", "dGhpcyBpcyBhIHRlc3Q=")
                    )
                ).AsString()
            );
        }
    }
}
