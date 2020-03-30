using Xunit;
using Yaapii.Http.Mock.Templates;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;

namespace Yaapii.Http.Mock.Test
{
    public sealed class MatchingWireTests
    {
        [Theory]
        [InlineData("first", "first expected result")]
        [InlineData("second", "second expected result")]
        public void RoutesToPath(string path, string expected)
        {
            Assert.Equal(
                expected,
                new Body.Of(
                    new MatchingWire(
                        new Match("first", req => "first expected result"),
                        new Match("second", req => "second expected result")
                    ).Response(
                        new Request(
                            new Path(path)
                        )
                    )
                ).AsString()
            );
        }

        [Theory]
        [InlineData("first", "first expected result")]
        [InlineData("second", "second expected result")]
        public void RoutesToHeader(string headerValue, string expected)
        {
            Assert.Equal(
                expected,
                new Body.Of(
                    new MatchingWire(
                        new Match("same/path",
                            new Header("important header", "first"),
                            req => "first expected result"
                        ),
                        new Match("same/path",
                            new Header("important header", "second"),
                            req => "second expected result"
                        )
                    ).Response(
                        new Request(
                            new Path("same/path"),
                            new Header("important header", headerValue)
                        )
                    )
                ).AsString()
            );
        }

        [Theory]
        [InlineData("first", "first expected result")]
        [InlineData("second", "second expected result")]
        public void RoutesToJoinedRequestPart(string QueryParamValue, string expected)
        {
            Assert.Equal(
                expected,
                new Body.Of(
                    new MatchingWire(
                        new Match("same/path", 
                            new Parts.Joined(
                                new Body("important data"),
                                new QueryParam("someParam", "first")
                            ), 
                            req => "first expected result"
                        ),
                        new Match("same/path",
                            new Parts.Joined(
                                new Body("important data"),
                                new QueryParam("someParam", "second")
                            ), 
                            req => "second expected result"
                        )
                    ).Response(
                        new Request(
                            new Path("same/path"),
                            new Body("important data"),
                            new QueryParam("someParam", QueryParamValue)
                        )
                    )
                ).AsString()
            );
        }

        [Fact]
        public void Returns404()
        {
            Assert.Equal(
                404,
                new Status.Of(
                    new MatchingWire(
                        new Match("first", req => "irrelevant"),
                        new Match("second", req => "more irrelevant")
                    ).Response(
                        new Request(
                            new Path("unknown/path")
                        )
                    )
                ).AsInt()
            );
        }
    }
}
