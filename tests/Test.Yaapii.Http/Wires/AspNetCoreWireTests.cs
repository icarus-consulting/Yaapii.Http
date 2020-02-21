using System;
using Xunit;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
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
    public sealed class AspNetCoreWireTests
    {
        [Fact]
        public void SendsRequest()
        {
            using (var server =
                new HttpMock(
                    new Kvp.Of<IWire>("test/asdf",
                        new FkWire()
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    200,
                    new Status.Of(
                        new AspNetCoreWire().Response(
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
            var hasHeader = false;
            using (var server =
                new HttpMock(
                    new FkWire(req =>
                    {
                        if(new FirstOf<string>(new Authorization.Of(req)).Value() == "Basic dXNlcjpwYXNzd29yZA==")
                        {
                            hasHeader = true;
                        }
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                new AspNetCoreWire().Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Header("Authorization", "Basic dXNlcjpwYXNzd29yZA==")
                    )
                );
            }
            Assert.True(hasHeader);
        }

        [Fact]
        public void SendsMultipleHeaderValues()
        {
            var hasHeader = false;
            using (var server =
                new HttpMock(
                    new FkWire(req =>
                    {
                        var accept = new Acccept.Of(req);
                        if (
                            new Contains<string>(accept, "aplication/json").Value() &&
                            new Contains<string>(accept, "aplication/xml").Value()
                        )
                        {
                            hasHeader = true;
                        }
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                new AspNetCoreWire().Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Headers(
                            new Kvp.Of("Accept", "aplication/json"),
                            new Kvp.Of("Accept", "aplication/xml"),
                            new Kvp.Of("Accept", "test/plain")
                        )
                    )
                );
            }
            Assert.True(hasHeader);
        }

        [Fact]
        public void ReturnsHeaders()
        {
            using (var server =
                new HttpMock(
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
                            new AspNetCoreWire().Response(
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
        [InlineData(0, "DELETE")]
        [InlineData(1, "GET")]
        [InlineData(2, "POST")]
        [InlineData(3, "PUT")]
        public void ReturnsMultipleHeaderValues(int index, string expected)
        {
            using (var server =
                new HttpMock(
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
                Assert.Equal(
                    expected,
                    new ItemAt<string>(
                        new Header.Of(
                            new AspNetCoreWire().Response(
                                new Get(
                                    new Scheme("http"),
                                    new Host("localhost"),
                                    new Port(server.Port)
                                )
                            ),
                            "Allow"
                        ),
                        index
                    ).Value()
                );
            }
        }

        [Fact]
        public void SendsBody()
        {
            var hasBody = false;
            using (var server =
                new HttpMock(
                    new FkWire(req =>
                    {
                        if (new Body.Of(req).AsString() == "very important content")
                        {
                            hasBody = true;
                        }
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                new AspNetCoreWire().Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new TextBody("very important content")
                    )
                );
            }
            Assert.True(hasBody);
        }

        [Fact]
        public void ReturnsBody()
        {
            using (var server =
                new HttpMock(
                    new FkWire(
                        new TextOf("very important content")
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    "very important content",
                    new Body.Of(
                        new AspNetCoreWire().Response(
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
                new AspNetCoreWire().Response(
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
                new AspNetCoreWire().Response(
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
                new AspNetCoreWire().Response(
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
                        new AspNetCoreWire(),
                        new Get(
                            "https://example.com"
                        )
                    )
                ).AsString()
            );
        }
    }
}
