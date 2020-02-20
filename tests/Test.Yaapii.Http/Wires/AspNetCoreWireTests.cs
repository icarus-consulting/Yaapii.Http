using MockHttpServer;
using System;
using Xunit;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;
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
                new MockServer(0, // pick random unused port
                    "test",
                    (req, res, prms) => { }
                )
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
                                new Path("test")
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
                new MockServer(0, // pick random unused port
                    "test",
                    (req, res, prms) => 
                    {
                        if(req.Headers["Authorization"] == "Basic dXNlcjpwYXNzd29yZA==")
                        {
                            hasHeader = true;
                        }
                    }
                )
            )
            {
                new AspNetCoreWire().Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Path("test"),
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
                new MockServer(0, // pick random unused port
                    "test",
                    (req, res, prms) =>
                    {
                        if (req.Headers["Accept"] == "aplication/json, aplication/xml, test/plain")
                        {
                            hasHeader = true;
                        }
                    }
                )
            )
            {
                new AspNetCoreWire().Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Path("test"),
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
                new MockServer(0, // pick random unused port
                    "test",
                    (req, res, prms) =>
                    {
                        res.Header("Allow", "GET");
                    }
                )
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
                                    new Port(server.Port),
                                    new Path("test")
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
                new MockServer(0, // pick random unused port
                    "test",
                    (req, res, prms) =>
                    {
                        res.Header("Allow", "DELETE");
                        res.Header("Allow", "GET");
                        res.Header("Allow", "POST");
                        res.Header("Allow", "PUT");
                    }
                )
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
                                    new Port(server.Port),
                                    new Path("test"),
                                    new Header("Authorization", "Basic dXNlcjpwYXNzd29yZA==")
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
                new MockServer(0, // pick random unused port
                    "test",
                    (req, res, prms) =>
                    {
                        if (req.Content() == "very important content")
                        {
                            hasBody = true;
                        }
                    }
                )
            )
            {
                new AspNetCoreWire().Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(server.Port),
                        new Path("test"),
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
                new MockServer(0, // pick random unused port
                    "test",
                    (req, res, prms) => "very important content"
                )
            )
            {
                Assert.Equal(
                    "very important content",
                    new Body.Of(
                        new AspNetCoreWire().Response(
                            new Get(
                                new Scheme("http"),
                                new Host("localhost"),
                                new Port(server.Port),
                                new Path("test")
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
