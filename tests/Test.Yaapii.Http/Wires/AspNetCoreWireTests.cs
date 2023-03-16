﻿//MIT License

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

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
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
using Yaapii.Http.Test;
using Yaapii.Http.Wires.AspNetCore;
using Yaapii.Web.Asp.Test;

namespace Yaapii.Http.Wires.Test
{
    [Collection("actual http tests")]
    public sealed class AspNetCoreWireTests
    {
        [Fact]
        public void SendsRequest()
        {
            var port = new AwaitedPort(new TestPort()).Value();
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
                                new Port(port),
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
            var port = new AwaitedPort(new TestPort()).Value();
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
                        new Port(port),
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
            var port = new AwaitedPort(new TestPort()).Value();
            IEnumerable<string> headers = new ManyOf<string>();
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                    {
                        headers = new Accept.Of(req);
                        headers.GetEnumerator().MoveNext(); // trigger lazy initialization before request gets disposed
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
                        new Port(port),
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
            var port = new AwaitedPort(new TestPort()).Value();
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
                                    new Port(port)
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
            var port = new AwaitedPort(new TestPort()).Value();
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
                                new Port(port)
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
            var port = new AwaitedPort(new TestPort()).Value();
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
                        new Port(port),
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
            var port = new AwaitedPort(new TestPort()).Value();
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
                                    new Port(port)
                                )
                            )
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
            try
            {
                new AspNetCoreWire(
                    new AspNetCoreClients(),
                    new TimeSpan(0, 1, 0)
                ).Response(
                    new Request(
                        new Method("unknownMethod"),
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
                        )
                    )
                ).AsString().ToLower()
            );
        }

        [Fact]
        public void EncodesSpecialCharactersInFormBody()
        {
            var port = new AwaitedPort(new TestPort()).Value();
            string body = "";
            using (var server =
                WebHost.CreateDefaultBuilder()
                    .UseKestrel((opt) =>
                    {
                        opt.ListenAnyIP(port);
                        opt.AllowSynchronousIO = true;
                    })
                    .Configure((app) =>
                        app.Run((httpContext) =>
                            Task.Run(() =>
                            {
                                body =
                                    new TextOf(
                                        httpContext.Request.Body
                                    ).AsString();
                            })
                        )
                    ).Start()
            )
            {
                new Verified(
                    new AspNetCoreWire(
                        new AspNetCoreClients(),
                        new TimeSpan(0, 1, 0)
                    ),
                    new ExpectedStatus(200)
                ).Response(
                    new Get(
                        new Scheme("http"),
                        new Host("localhost"),
                        new Port(port),
                        new FormParam("key-name", "test&+=xyz"),
                        new BearerTokenAuth("Bearer sdfnhiausihfnksajn")
                    )
                );
            }
            Assert.Equal(
                "key-name=test%26%2B%3Dxyz",
                body
            );
        }

        [Fact]
        public void FormBodyWorksWithKestrel()
        {
            var body = "";
            var port = new AwaitedPort(new TestPort()).Value();
            var host = WebHost.CreateDefaultBuilder();

            host.UseUrls($"http://{Environment.MachineName.ToLower()}:{port}");
            host.UseKestrel((opt) => opt.AllowSynchronousIO = true);
            host.ConfigureServices(svc =>
            {
                svc.AddSingleton<Action<HttpRequest>>(req => // required to instantiate HtAction from dependecy injection
                {
                    body = new TextOf(req.Body).AsString();
                });
                svc
                    .AddMvc()
                    .AddApplicationPart(
                        typeof(HtAction).Assembly
                    )
                    .AddControllersAsServices();
            });
            host.Configure(app =>
            {
                app
#if NET6_0
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());
#else
                .UseMvc();
#endif
            });

            using (var built = host.Build())
            {
                built.RunAsync();
                try
                {
                    new Verified(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ),
                        new ExpectedStatus(200)
                    ).Response(
                        new Post(
                            new Scheme("http"),
                            new Host("localhost"),
                            new Port(port),
                            new Path("action"),
                            new FormParams(
                                new MapOf(
                                    "key-1-name", "this is a test",
                                    "key-2-name", "test&+=xyz"
                                )
                            )
                        )
                    );
                }
                finally
                {
                    built.StopAsync();
                }
            }

            Assert.Equal(
                "key-1-name=this+is+a+test&key-2-name=test%26%2B%3Dxyz",
                body
            );
        }

        [Fact]
        public void FormBodyDoesNotAddDuplicateHeaders()
        {
            var contentTypeHeaders = new List<string>();
            var port = new AwaitedPort(new TestPort()).Value();
            var host = WebHost.CreateDefaultBuilder();

            host.UseUrls($"http://{Environment.MachineName.ToLower()}:{port}");
            host.ConfigureServices(svc =>
            {
                svc.AddSingleton<Action<HttpRequest>>(req => // required to instantiate HtAction from dependecy injection
                {
                    foreach (var header in req.Headers["Content-Type"])
                    {
                        contentTypeHeaders.AddRange(
                            new Split(header, ", ")
                        );
                    }
                });
                svc.AddMvc().AddApplicationPart(
                    typeof(HtAction).Assembly
                ).AddControllersAsServices();
            });
            host.Configure(app =>
            {
                app
#if NET6_0
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());
#else
                .UseMvc();
#endif
            });

            using (var built = host.Build())
            {
                built.RunAsync();
                try
                {
                    new Verified(
                        new AspNetCoreWire(
                            new AspNetCoreClients(),
                            new TimeSpan(0, 1, 0)
                        ),
                        new ExpectedStatus(200)
                    ).Response(
                        new Post(
                            new Scheme("http"),
                            new Host("localhost"),
                            new Port(port),
                            new Path("action"),
                            new FormParams(
                                new MapOf(
                                    "key-1-name", "this is a test",
                                    "key-2-name", "test&+=xyz"
                                )
                            )
                        )
                    );
                }
                finally
                {
                    built.StopAsync();
                }
            }

            Assert.Single(
                contentTypeHeaders
            );
        }
    }
}
