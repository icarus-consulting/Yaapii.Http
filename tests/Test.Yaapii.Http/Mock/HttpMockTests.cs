using System;
using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;
using Yaapii.Http.Wires;
using Yaapii.Web.Asp;

namespace Yaapii.Http.Mock.Test
{
    public sealed class HttpMockTests
    {
        [Fact]
        public void ListensToAnyPath()
        {
            var port = new RandomPort().Value();
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
                new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
                    new Get($"http://localhost:{port}")
                );
                new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
                    new Get($"http://localhost:{port}/path")
                );
                new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
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
            var port = new RandomPort().Value();
            var result = 0;
            using (var server =
                new HttpMock(port,
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
                new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
                    new Get($"http://localhost:{port}")
                );
                new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
                    new Get($"http://localhost:{port}/path")
                );
                new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
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
            var port = new RandomPort().Value();
            using (var server =
                new HttpMock(port,
                    new FkWire()
                ).Value()
            )
            {
                Assert.Equal(
                    200,
                    new Status.Of(
                        new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
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
            var port = new RandomPort().Value();
            using (var server =
                new HttpMock(port,
                    new Kvp.Of<IWire>("path",
                        new FkWire()
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    404,
                    new Status.Of(
                        new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
                            new Get($"http://localhost:{port}/unknown/path")
                        )
                    ).AsInt()
                );
            }
        }

        [Fact]
        public void ForwardsQueryParams()
        {
            var port = new RandomPort().Value();
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
                new AspNetCoreWire(new TimeSpan(0, 5, 0)).Response(
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
