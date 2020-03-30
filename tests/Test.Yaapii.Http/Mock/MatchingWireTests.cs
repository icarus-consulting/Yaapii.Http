//MIT License

//Copyright(c) 2020 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

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
