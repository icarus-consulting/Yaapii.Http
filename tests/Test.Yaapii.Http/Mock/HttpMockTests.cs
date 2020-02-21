﻿using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;
using Yaapii.Http.Wires;

namespace Yaapii.Http.Mock.Test
{
    public sealed class HttpMockTests
    {
        [Fact]
        public void ListensToAnyPath()
        {
            var requests = 0;
            using (var server =
                new HttpMock(
                    new FkWire(req =>
                    {
                        requests++;
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                var port = server.Port;
                new AspNetCoreWire().Response(
                    new Get($"http://localhost:{port}")
                );
                new AspNetCoreWire().Response(
                    new Get($"http://localhost:{port}/path")
                );
                new AspNetCoreWire().Response(
                    new Get($"http://localhost:{port}/another/path")
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
            var result = 0;
            using (var server =
                new HttpMock(
                    new Kvp.Of<IWire>("", 
                        new FkWire(req =>
                        {
                            result += 1;
                            return new Response.Of(200, "OK");
                        })
                    ),
                    new Kvp.Of<IWire>("path",
                        new FkWire(req =>
                        {
                            result += 2;
                            return new Response.Of(200, "OK");
                        })
                    ),
                    new Kvp.Of<IWire>("another/path",
                        new FkWire(req =>
                        {
                            result += 4;
                            return new Response.Of(200, "OK");
                        })
                    )
                ).Value()
            )
            {
                var port = server.Port;
                new AspNetCoreWire().Response(
                    new Get($"http://localhost:{port}")
                );
                new AspNetCoreWire().Response(
                    new Get($"http://localhost:{port}/path")
                );
                new AspNetCoreWire().Response(
                    new Get($"http://localhost:{port}/another/path")
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
            using (var server =
                new HttpMock(
                    new FkWire()
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
        public void Returns404()
        {
            using (var server =
                new HttpMock(
                    new Kvp.Of<IWire>("path",
                        new FkWire()
                    )
                ).Value()
            )
            {
                var port = server.Port;
                Assert.Equal(
                    404,
                    new Status.Of(
                        new AspNetCoreWire().Response(
                            new Get($"http://localhost:{port}/unknown/path")
                        )
                    ).AsInt()
                );
            }
        }

        [Fact]
        public void ForwardsQueryParams()
        {
            var queryParam = "";
            using (var server =
                new HttpMock(
                    new FkWire(req =>
                    {
                        queryParam = new QueryParam.Of(req, "importantQueryParam").AsString();
                        return new Response.Of(200, "OK");
                    })
                ).Value()
            )
            {
                var port = server.Port;
                new AspNetCoreWire().Response(
                    new Get($"http://localhost:{port}?importantQueryParam=importantValue")
                );
            }
            Assert.Equal(
                "importantValue",
                queryParam
            );
        }
    }
}
