using Xunit;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class TextBodyOfTests
    {
        [Fact]
        public void ReadsText()
        {
            var expected = "this is a text";
            Assert.Equal(
                expected,
                new TextBody.Of(
                    new Request(
                        new Body(expected)
                    )
                ).AsString()
            );
        }
    }
}
