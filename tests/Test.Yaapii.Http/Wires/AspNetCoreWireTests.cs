using System;
using System.Collections.Generic;
using Xunit;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Scalar;
using Yaapii.Http.AtomsTemp.Text;
using Yaapii.Http.Fake;
using Yaapii.Http.Mock;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;

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
                    new Kvp.Of<IWire>("test/asdf",
                        new FkWire()
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    200,
                    new Status.Of(
                        new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
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
                new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Header("Authorization", "Basic dXNlcjpwYXNzd29yZA==")
                    )
                );
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
            IEnumerable<string> headers = new Many.Of<string>();
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
                new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Headers(
                            new Kvp.Of("Accept", "application/json"),
                            new Kvp.Of("Accept", "application/xml"),
                            new Kvp.Of("Accept", "text/plain")
                        )
                    )
                );
            }
            Assert.Contains(
                expected,
                new Yaapii.Http.AtomsTemp.Text.Joined(", ", headers).AsString()
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
                                new Many.Of<IKvp>(
                                    new Kvp.Of("Allow", "GET")
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
                            new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                                new Get(
                                    new Scheme("http"),
                                    new Host("localhost"),
                                    new Port(server.Port)
                                )
                            ),
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
                                new Many.Of<IKvp>(
                                    new Kvp.Of("Allow", "DELETE"),
                                    new Kvp.Of("Allow", "GET"),
                                    new Kvp.Of("Allow", "POST"),
                                    new Kvp.Of("Allow", "PUT")
                                )
                            );
                    })
                ).Value()
            )
            {
                Assert.Contains(
                    expected,
                    new Header.Of(
                        new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                            new Get(
                                new Scheme("http"),
                                new Host("localhost"),
                                new Port(server.Port)
                            )
                        ),
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
                        body = new Body.Of(req).AsString();
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new TextBody("very important content")
                    )
                );
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
                    new Body.Of(
                        new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                            new Get(
                                new Scheme("http"),
                                new Host("localhost"),
                                new Port(server.Port)
                            )
                        )
                    ).AsString()
                );
            }
        }

        [Fact]
        public void RejectsMissingMethod()
        {
            Assert.Throws<ArgumentException>(() =>
                new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                    new Requests.Request(
                        new Address("http://localhost")
                    )
                )
            );
        }

        [Fact]
        public void RejectsUnknownMethod()
        {
            Assert.Throws<ArgumentException>(() =>
                new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                    new Requests.Request(
                        new Method("unknownMethod"),
                        new Address("http://localhost")
                    )
                )
            );
        }

        [Fact]
        public void RejectsMissingAddress()
        {
            Assert.Throws<ArgumentException>(() =>
                new AspNetCoreWire(new TimeSpan(0, 1, 0)).Response(
                    new Get()
                )
            );
        }

        [Fact]
        public void GetsWebsite()
        {
            Assert.StartsWith(
                "<!doctype html>",
                new Body.Of(
                    new Response(
                        new AspNetCoreWire(new TimeSpan(0, 1, 0)),
                        new Get(
                            "https://example.com"
                        )
                    )
                ).AsString()
            );
        }
    }
}
