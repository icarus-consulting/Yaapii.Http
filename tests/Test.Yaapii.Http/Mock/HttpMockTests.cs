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
using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms.Map;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;
using Yaapii.Http.Wires;
using Yaapii.Http.Wires.AspNetCore;

namespace Yaapii.Http.Mock.Test
{
    [Collection("actual http tests")]
    public sealed class HttpMockTests
    {
        [Fact]
        public void ListensToAnyPath()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
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
            var port = new AwaitedPort(new RandomPort().Value()).Value();
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
            var port = new AwaitedPort(new RandomPort().Value()).Value();
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
            var port = new AwaitedPort(new RandomPort().Value()).Value();
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
            var port = new AwaitedPort(new RandomPort().Value()).Value();
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
    }
}
