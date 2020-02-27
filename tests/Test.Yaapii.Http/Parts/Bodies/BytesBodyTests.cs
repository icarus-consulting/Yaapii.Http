using Xunit;
using Yaapii.Http.AtomsTemp.Bytes;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class BytesBodyTests
    {
        [Fact]
        public void EncodesBase64()
        {
            Assert.Equal(
                "dGhpcyBpcyBhIHRlc3Q=",
                new Request(
                    new BytesBody(
                        new BytesOf(
                            "this is a test"
                        )
                    )
                )["body"]
            );
        }
    }
}
