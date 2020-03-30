using Xunit;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Mock.Templates.Test
{
    public sealed class JoinedTests
    {
        [Fact]
        public void ReturnsPrimaryTemplateResponse()
        {
            Assert.Equal(
                "correct response",
                new Body.Of(
                    new Joined(
                        new Conditional(
                            req => true,
                            new FkWire("correct response")
                        ),
                        new Conditional(
                            req => true,
                            new FkWire("wrong response")
                        )
                    ).Response(
                        new Request()
                    )
                ).AsString()
            );
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(true, true, true)]
        public void ChecksAllTemplates(bool expected, bool firstTemplateMatches, bool secondTemplateMatches)
        {
            Assert.Equal(
                expected,
                new Joined(
                    new Conditional(
                        req => firstTemplateMatches,
                        new FkWire()
                    ),
                    new Conditional(
                        req => secondTemplateMatches,
                        new FkWire()
                    )
                ).Applies(
                    new Request()
                )
            );
        }
    }
}
