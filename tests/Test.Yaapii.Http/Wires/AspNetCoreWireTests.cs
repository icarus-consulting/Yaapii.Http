﻿using System;
using System.Collections.Generic;
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
            var header = "";
            using (var server =
                new HttpMock(
                    new FkWire(req =>
                    {
                        header = new FirstOf<string>(new Authorization.Of(req)).Value();
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
            Assert.Equal(
                "Basic dXNlcjpwYXNzd29yZA==",
                header
            );
        }

        [Theory]
        [InlineData("aplication/json")]
        [InlineData("aplication/xml")]
        public void SendsMultipleHeaderValues(string expected)
        {
            IEnumerable<string> headers = new Many.Of<string>();
            using (var server =
                new HttpMock(
                    new FkWire(req =>
                    {
                        headers = new Acccept.Of(req);
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
            Assert.Contains(
                expected,
                headers
            );
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
        [InlineData("DELETE")]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        public void ReturnsMultipleHeaderValues(string expected)
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
                Assert.Contains(
                    expected,
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
                );
            }
        }

        [Fact]
        public void SendsBody()
        {
            var body = "";
            using (var server =
                new HttpMock(
                    new FkWire(req =>
                    {
                        body = new Body.Of(req).AsString();
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
            Assert.Equal(
                "very important content",
                body
            );
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
