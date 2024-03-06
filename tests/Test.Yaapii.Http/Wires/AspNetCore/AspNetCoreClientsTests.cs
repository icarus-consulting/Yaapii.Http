//MIT License

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

using System;
using System.IO.Compression;
using System.Net;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Text;
using Yaapii.Http.Fake;
using Yaapii.Http.Mock;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Uri;
using Yaapii.Http.Requests;
using Yaapii.Http.Test;

namespace Yaapii.Http.Wires.AspNetCore.Test
{
    [Collection("actual http tests")]
    public sealed class AspNetCoreClientsTests
    {
        [Fact]
        public void ReusesClients()
        {
            var clients = new AspNetCoreClients();
            var first =
                clients.Client(
                    new TimeSpan(0, 1, 0)
                );
            first.BaseAddress = new Uri("http://localhost/this/is/a/test");
            var second =
                clients.Client(
                    new TimeSpan(0, 1, 0) // same timeout should return same client
                );
            Assert.Equal(
                first.BaseAddress,
                second.BaseAddress
            );
        }

        [Fact]
        public void ReturnsDifferentClients()
        {
            var clients = new AspNetCoreClients();
            var first =
                clients.Client(
                    new TimeSpan(0, 1, 1)
                );
                first.BaseAddress = new Uri("http://localhost/this/is/a/test");
            var second =
                clients.Client(
                    new TimeSpan(0, 2, 1) // different timeout should return new client
                );
            Assert.NotEqual(
                first.BaseAddress,
                second.BaseAddress
            );
        }

        [Fact]
        public void SetsDefaultSecurityTypes()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            new AspNetCoreClients().Client(new TimeSpan(0, 1, 0));
            Assert.Equal(
                ServicePointManager.SecurityProtocol,
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
            );
        }

        [Fact]
        public void SetsDefaultDecompressionMethods()
        {
            var port = new AwaitedPort(new TestPort()).Value();

            var compressedStream = new System.IO.MemoryStream();
            var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
            new InputOf("very important content").Stream().CopyTo(zipStream);
            zipStream.Close();

            using (var server =
                new HttpMock(port,
                    new FkWire(
                        new MapOf("Content-Encoding", "gzip"),
                        new Body(
                            new InputOf(compressedStream.ToArray())
                        )
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
    }
}
