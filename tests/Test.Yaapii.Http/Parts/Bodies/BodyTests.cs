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

using MockHttpServer;
using Newtonsoft.Json.Linq;
using Xunit;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Yaapii.Atoms.Map;
using Yaapii.Http.Fake;
using Yaapii.Http.Mock;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;
using Yaapii.Http.Wires;
using Yaapii.Http.Wires.AspNetCore;
using Yaapii.Xml;

namespace Yaapii.Http.Parts.Bodies.Test
{
    [Collection("actual http tests")]
    public sealed class BodyTests
    {
        [Fact]
        public void WritesBytes()
        {
            var result = new byte[] { 1, 2, 0, 3 };

            Assert.Equal(
                result,
                new BytesOf(
                    new Body.Of(
                        new Body(result).Apply(
                            new MapOf(new MapInputOf())
                        )
                    )
                ).AsBytes()
            );
        }


        [Fact]
        public void WritesBody()
        {
            Assert.Equal(
                "| <-- stick figure body",
                new TextOf(
                    new Body.Of(
                        new Body(
                            new InputOf("| <-- stick figure body")
                        ).Apply(
                            new MapOf(new MapInputOf())
                        )
                    )
                ).AsString()
            );
        }

        [Fact]
        public void WritesJsonContentType()
        {
            Assert.Equal(
                "application/json",
                new Body(new JObject()).Apply(
                    new MapOf(new MapInputOf())
                )["header:0:Content-Type"]
            );
        }

        [Fact]
        public void WritesJsonObject()
        {
            Assert.Equal(
                new JObject(
                    new JProperty("key", "value")
                ).ToString(),
                new TextOf(
                    new Body.Of(
                        new MapOf(
                            new Body(
                                new JObject(
                                    new JProperty("key", "value")
                                )
                            )
                        )
                    )
                ).AsString()
            );
        }

        [Fact]
        public void WritesXmlContentType()
        {
            Assert.Equal(
                "application/xml",
                new Body(new XMLCursor("<irrelevant />")).Apply(
                    new MapOf(new MapInputOf())
                )["header:0:Content-Type"]
            );
        }

        [Fact]
        public void WritesXmlBody()
        {
            Assert.Equal(
                "<importantData />",
                new TextOf(
                    new Body.Of(
                        new Body(new XMLCursor("<importantData />")).Apply(
                            new MapOf(new MapInputOf())
                        )
                    )
                ).AsString()
            );
        }
        
        [Fact]
        public void TransmitsZipFile()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                        new Response.Of(
                            new Status(200),
                            new Reason("OK"),
                            new Body(
                                new ResourceOf(
                                    "Assets/test.zip",
                                    this.GetType().Assembly
                                ),
                                "application/zip"
                            )
                        )
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    "this is a test", // content of test.txt in Assets/test.zip
                    new TextOf(
                        new UnzippedFile(
                            new Body.Of(
                                new AspNetCoreWire(
                                    new AspNetCoreClients()
                                ).Response(
                                    new Get($"http://localhost:{port}/")
                                )
                            ),
                            "test.txt"
                        )
                    ).AsString()
                );
            }
        }

        [Fact]
        public void TransmitsRawZipFile()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            IInput result = new DeadInput();
            using (var server =
                new MockServer(
                    port,
                    "{}",
                    (req, res, prm) => 
                        result = 
                            new InputOf(
                                new BytesOf(
                                    new InputOf(req.InputStream)
                                ).AsBytes()
                            ),
                    "localhost"
                )
            )
            {
                new AspNetCoreWire(
                    new AspNetCoreClients()
                ).Response(
                    new Post($"http://localhost:{port}/",
                        new Body(
                            new ResourceOf(
                                "Assets/test.zip",
                                this.GetType().Assembly
                            )
                        )
                    )
                );
            }
            Assert.Equal(
                "this is a test", // content of test.txt in Assets/test.zip
                new TextOf(
                    new UnzippedFile(result, "test.txt")
                ).AsString()
            );

        }
    }
}
