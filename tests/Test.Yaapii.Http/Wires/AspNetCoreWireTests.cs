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

using System;
using System.Collections.Generic;
using Xunit;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Http.Fake;
using Yaapii.Http.Mock;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;
using Yaapii.Http.Wires.AspNetCore;

namespace Yaapii.Http.Wires.Test
{
    [Collection("actual http tests")]
    public sealed class AspNetCoreWireTests
    {
        [Fact]
        public void SendsRequest()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            using (var server =
                new HttpMock(port,
                    new KvpOf<IWire>("test/asdf",
                        new FkWire()
                    )
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
                                new Port(server.Port),
                                new Path("test/asdf")
                            )
                        )
                    ).AsInt()
                );
            }
        }

        [Fact]
        public void SendsHeaders()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            var header = "";
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        header = new FirstOf<string>(new Authorization.Of(req)).Value();
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                new AspNetCoreWire(
                    new AspNetCoreClients(),
                    new TimeSpan(0, 1, 0)
                ).Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Header("Authorization", "Basic dXNlcjpwYXNzd29yZA==")
                    )
                ).Wait(30000);
            }
            Assert.Equal(
                "Basic dXNlcjpwYXNzd29yZA==",
                header
            );
        }

        [Theory]
        [InlineData("application/json")]
        [InlineData("application/xml")]
        [InlineData("text/plain")]
        public void SendsMultipleHeaderValues(string expected)
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            IEnumerable<string> headers = new ManyOf<string>();
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        headers = new Accept.Of(req);
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                new AspNetCoreWire(
                    new AspNetCoreClients(),
                    new TimeSpan(0, 1, 0)
                ).Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Headers(
                            new KvpOf("Accept", "application/json"),
                            new KvpOf("Accept", "application/xml"),
                            new KvpOf("Accept", "text/plain")
                        )
                    )
                ).Wait(30000);
            }
            Assert.Contains(
                expected,
                new Yaapii.Atoms.Text.Joined(", ", headers).AsString()
            );
        }

        [Fact]
        public void ReturnsHeaders()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        return
                            new Response.Of(200, "OK",
                                new ManyOf<IKvp>(
                                    new KvpOf("Allow", "GET")
                                )
                            );
                    })
                ).Value()
            )
            {
                Assert.Equal(
                    "GET",
                    new FirstOf<string>(
                        new Header.Of(
                            new AspNetCoreWire(
                                new AspNetCoreClients(),
                                new TimeSpan(0, 1, 0)
                            ).Response(
                                new Get(
                                    new Scheme("http"),
                                    new Host("localhost"),
                                    new Port(server.Port)
                                )
                            ).Result,
                            "Allow"
                        )
                    ).Value()
                );
            }
        }

        [Theory]
        [InlineData("DELETE")]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        public void ReturnsMultipleHeaderValues(string expected)
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        return
                            new Response.Of(200, "OK",
                                new ManyOf<IKvp>(
                                    new KvpOf("Allow", "DELETE"),
                                    new KvpOf("Allow", "GET"),
                                    new KvpOf("Allow", "POST"),
                                    new KvpOf("Allow", "PUT")
                                )
                            );
                    })
                ).Value()
            )
            {
                Assert.Contains(
                    expected,
                    new Header.Of(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ).Response(
                            new Get(
                                new Scheme("http"),
                                new Host("localhost"),
                                new Port(server.Port)
                            )
                        ).Result,
                        "Allow"
                    )
                );
            }
        }

        [Fact]
        public void SendsBody()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            var body = "";
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        body =
                            new TextOf(
                                new Body.Of(req)
                            ).AsString();
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                new AspNetCoreWire(
                    new AspNetCoreClients(),
                    new TimeSpan(0, 1, 0)
                ).Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new TextBody("very important content")
                    )
                ).Wait(30000);
            }
            Assert.Equal(
                "very important content",
                body
            );
        }

        [Fact]
        public void ReturnsBody()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(
                        new TextOf("very important content")
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    "very important content",
                    new TextOf(
                        new Body.Of(
                            new AspNetCoreWire(
                                new AspNetCoreClients(),
                                new TimeSpan(0, 1, 0)
                            ).Response(
                                new Get(
                                    new Scheme("http"),
                                    new Host("localhost"),
                                    new Port(server.Port)
                                )
                            ).Result
                        )
                    ).AsString()
                );
            }
        }

        [Fact]
        public void RejectsMissingMethod()
        {
            try
            {
                new AspNetCoreWire(
                    new AspNetCoreClients(),
                    new TimeSpan(0, 1, 0)
                ).Response(
                    new Request(
                        new Address("http://localhost")
                    )
                ).Wait(30000);
            }
            catch (AggregateException agg)
            {
                Assert.True(agg.InnerException is ArgumentException);
            }
        }

        [Fact]
        public void RejectsUnknownMethod()
        {
            Assert.Throws<ArgumentException>(() =>
                new AspNetCoreWire(
                    new AspNetCoreClients(),
                    new TimeSpan(0, 1, 0)
                ).Response(
                    new Requests.Request(
                        new Method("unknownMethod"),
                        new Address("http://localhost")
                    )
                ).Result
            );
        }

        [Fact]
        public void RejectsMissingAddress()
        {
            try
            {
                new AspNetCoreWire(
                    new AspNetCoreClients(),
                    new TimeSpan(0, 1, 0)
                ).Response(
                    new Get()
                ).Wait();
            }
            catch (AggregateException agg)
            {
                Assert.True(agg.InnerException is ArgumentException);
            }
        }

        [Fact]
        public void GetsWebsite()
        {
            Assert.StartsWith(
                "<!doctype html>",
                new TextOf(
                    new Body.Of(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ).Response(
                            new Get(
                                "https://google.com"
                            )
                        ).Result
                    )
                ).AsString()
            );
        }
    }
}
