using Xunit;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class BytesBodyOfTests
    {
        [Fact]
        public void ReadsXml()
        {
            var expected = new byte[] { 0, 1, 2, 3 };
            Assert.Equal(
                expected,
                new BytesBody.Of(
                    new Request(
                        new Body(expected)
                    )
                ).AsBytes()
            );
        }
    }
}
