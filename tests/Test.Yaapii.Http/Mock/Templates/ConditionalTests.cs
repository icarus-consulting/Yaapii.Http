using Xunit;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Mock.Templates.Test
{
    public sealed class ConditionalTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AppliesOnCondition(bool expected)
        {
            Assert.Equal(
                expected,
                new Conditional(
                    req => expected,
                    new FkWire()
                ).Applies(new Request())
            );
        }

        [Fact]
        public void HasResponse()
        {
            var expected = "expected response";
            Assert.Equal(
                expected,
                new Body.Of(
                    new Conditional(
                        req => true,
                        new FkWire(expected)
                    ).Response(new Request())
                ).AsString()
            );
        }
    }
}
