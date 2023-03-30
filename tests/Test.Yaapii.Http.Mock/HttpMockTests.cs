//MIT License

//Copyright(c) 2023 ICARUS Consulting GmbH

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

using Nito.AsyncEx;
using System;
using System.Threading.Tasks;
using Test.Yaapii.Http.Mock;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;
using Yaapii.Http.Wires;
using Yaapii.Http.Wires.AspNetCore;
using Yaapii.Xml;

namespace Yaapii.Http.Mock.Test
{
    [Collection("actual http tests")]
    public sealed class HttpMockTests
    {
        [Fact]
        public void ListensToAnyPath()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            var clients = new AspNetCoreClients();
            var requests = 0;
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        requests++;
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                Task.WaitAll(
                    new AspNetCoreWire(clients, new TimeSpan(0, 1, 0)).Response(
                        new Get($"http://localhost:{port}")
                    ),
                    new AspNetCoreWire(clients, new TimeSpan(0, 1, 0)).Response(
                        new Get($"http://localhost:{port}/path")
                    ),
                    new AspNetCoreWire(clients, new TimeSpan(0, 1, 0)).Response(
                        new Get($"http://localhost:{port}/another/path")
                    )
                );
            }
            Assert.Equal(
                3,
                requests
            );
        }

        [Fact]
        public void RoutesToPath()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            var clients = new AspNetCoreClients();
            var result = 0;
            using (var server =
                new HttpMock(port,
                    new KvpOf<IWire>("",
                        new FkWire(req =>
                        {
                            result += 1;
                            return new Response.Of(200, "OK");
                        })
                    ),
                    new KvpOf<IWire>("path",
                        new FkWire(req =>
                        {
                            result += 2;
                            return new Response.Of(200, "OK");
                        })
                    ),
                    new KvpOf<IWire>("another/path",
                        new FkWire(req =>
                        {
                            result += 4;
                            return new Response.Of(200, "OK");
                        })
                    )
                ).Value()
            )
            {
                Task.WaitAll(
                    new AspNetCoreWire(clients, new TimeSpan(0, 1, 0)).Response(
                        new Get($"http://localhost:{port}")
                    ),
                    new AspNetCoreWire(clients, new TimeSpan(0, 1, 0)).Response(
                        new Get($"http://localhost:{port}/path")
                    ),
                    new AspNetCoreWire(clients, new TimeSpan(0, 1, 0)).Response(
                        new Get($"http://localhost:{port}/another/path")
                    )
                );
            }
            Assert.Equal(
                7,
                result
            );
        }

        [Fact]
        public void Returns200()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire()
                ).Value()
            )
            {
                Assert.Equal(
                    200,
                    new Status.Of(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ).Response(
                            new Get(
                                new Scheme("http"),
                                new Host("localhost"),
                                new Port(port),
                                new Path("test/asdf")
                            )
                        )
                    ).AsInt()
                );
            }
        }

        [Fact]
        public void Returns404()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            using (var server =
                new HttpMock(port,
                    new KvpOf<IWire>("path",
                        new FkWire()
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    404,
                    new Status.Of(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ).Response(
                            new Get($"http://localhost:{port}/unknown/path")
                        )
                    ).AsInt()
                );
            }
        }

        [Fact]
        public void ForwardsQueryParams()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            var queryParam = "";
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        queryParam = new QueryParam.Of(req, "importantQueryParam").AsString();
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                new AspNetCoreWire(
                    new AspNetCoreClients(),
                    new TimeSpan(0, 1, 0)
                ).Response(
                    new Get($"http://localhost:{port}?importantQueryParam=importantValue")
                ).Wait(30000);
            }
            Assert.Equal(
                "importantValue",
                queryParam
            );
        }

        [Fact]
        public void DeliversXmlResponse()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        return new Response.Of(new Body(new XMLCursor(new InputOf("<test/>"))));
                    })
                ).Value()
            )
            {
                var response =
                    AsyncContext.Run(() =>
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ).Response(
                             new Get($"http://localhost:{port}?importantQueryParam=importantValue")
                        )
                    );

                Assert.Equal(
                    "<test />",
                    response.HasBody()
                    ? new XmlBody.Of(response).AsNode().ToString()
                    : "response doesn't have a body"
                );
            }
        }

        [Fact]
        public void ReturnsMultipleHeaderValues()
        {
            var header = "some-header-name";
            var port = new AwaitedPort(new TestPort()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(
                        new Header(header, "value1"),
                        new Header(header, "value2")
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    new ManyOf("value1", "value2"),
                    new Header.Of(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ).Response(
                             new Get($"http://localhost:{port}")
                        ),
                        header
                    )
                );
            }
        }

        [Fact]
        public void ReturnsReasonPhrase()
        {
            var reason = "because we can";
            var port = new AwaitedPort(new TestPort()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(
                        200,
                        reason
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    reason,
                    new Reason.Of(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ).Response(
                             new Get($"http://localhost:{port}")
                        )
                    ).AsString()
                );
            }
        }

        [Fact]
        public void ReturnsInternalErrors()
        {
            var errorMessage = "I'm sorry Dave, i'm afraid i can not do that.";
            var port = new AwaitedPort(new TestPort()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(requestAction:
                        (req) => throw new InvalidOperationException(errorMessage)
                    )
                ).Value()
            )
            {
                Assert.Contains(
                    errorMessage,
                    new TextBody.Of(
                        new Verified(
                            new AspNetCoreWire(
                                new AspNetCoreClients(),
                                new TimeSpan(0, 1, 0)
                            ),
                            new ExpectedStatus(500)
                        ).Response(
                            new Get($"http://localhost:{port}")
                        )
                    ).AsString()
                );
            }
        }

        [Fact]
        public void BuildsRequestAddress()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            var result = "";
            using (var server =
                new HttpMock(port,
                    new FkWire(requestAction: (req) =>
                    {
                        result = new Address.Of(req).Value().ToString();
                    })
                ).Value()
            )
            {
                var expected = $"http://localhost:{port}/t/e/s/t?param1=value1&param2=value2";
                new Verified(
                    new AspNetCoreWire(
                        new AspNetCoreClients(),
                        new TimeSpan(0, 1, 0)
                    ),
                    new ExpectedStatus(200)
                ).Response(
                    new Get(expected)
                );
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void ReturnsBodyMoreThanOnce()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire("test")
                ).Value()
            )
            {
                var result = "";
                result +=
                    new TextBody.Of(
                        new Verified(
                            new AspNetCoreWire(
                                new AspNetCoreClients(),
                                new TimeSpan(0, 1, 0)
                            ),
                            new ExpectedStatus(200)
                        ).Response(
                            new Get($"http://localhost:{port}")
                        )
                    ).AsString();
                result +=
                    new TextBody.Of(
                        new Verified(
                            new AspNetCoreWire(
                                new AspNetCoreClients(),
                                new TimeSpan(0, 1, 0)
                            ),
                            new ExpectedStatus(200)
                        ).Response(
                            new Get($"http://localhost:{port}")
                        )
                    ).AsString();
                Assert.Equal(
                    "testtest",
                    result
                );
            }
        }

        [Fact(Skip = "Appveyor doesn't have a default certificate to use for HTTPS.")]
        public void UsesHttps()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(),
                    useHttps: true
                ).Value()
            )
            {
                Assert.Equal(
                    200,
                    new Status.Of(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ).Response(
                            new Get(
                                new Scheme("https"),
                                new Host("localhost"),
                                new Port(port),
                                new Path("test/asdf")
                            )
                        )
                    ).AsInt()
                );
            }
        }
    }
}
