using Xunit;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Mock.Templates.Test
{
    public sealed class MatchTests
    {
        [Fact]
        public void HasResponse()
        {
            var expected = "expected response";
            Assert.Equal(
                expected,
                new Body.Of(
                    new Match(
                        "path",
                        new FkWire(expected)
                    ).Response(
                        new Request()
                    )
                ).AsString()
            );
        }

        [Fact]
        public void DoesNotApplyForMissingParts()
        {
            Assert.False(
                new Match(
                    new Method("get"),
                    new FkWire()
                ).Applies(
                    new Request() // has no method part
                )
            );
        }

        [Fact]
        public void AppliesForMatchingParts()
        {
            Assert.True(
                new Match(
                    new Parts.Joined(
                        new Method("get"),
                        new Path("some/path"),
                        new QueryParam("someParam", "someValue"),
                        new BearerTokenAuth("a valid token"),
                        new Body("important data")
                    ),
                    new FkWire()
                ).Applies(
                    new Get(
                        "http://localhost/some/path?someParam=someValue",
                        new BearerTokenAuth("a valid token"),
                        new Body("important data")
                    )
                )
            );
        }
    }
}
