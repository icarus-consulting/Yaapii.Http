﻿//MIT License

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

using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Fake;
using Yaapii.Http.Mock;
using Yaapii.Http.Requests;
using Yaapii.Http.Responses;
using Yaapii.Http.Wires;
using Yaapii.Http.Wires.AspNetCore;

namespace Yaapii.Http.Parts.Bodies.Test
{
    [Collection("actual http tests")]
    public sealed class TextBodyTests
    {
        [Fact]
        public void WritesContentType()
        {
            Assert.Equal(
                "text/plain",
                new TextBody("irrelevant").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Content-Type"]
            );
        }

        [Fact]
        public void WritesBody()
        {
            Assert.Equal(
                "some body",
                new TextBody.Of(
                    new TextBody("some body").Apply(
                        new Map.Of(new MapInput.Of())
                    )
                ).AsString()
            );
        }

        [Fact]
        public void TransmitsLongText()
        {
            var port = new AwaitedPort(new RandomPort().Value()).Value();
            using (var server =
                new HttpMock(port,
                    new FkWire(req =>
                        new Response.Of(
                            new Status(200),
                            new Reason("OK"),
                            new TextBody(
                                new TextOf(
                                    new ResourceOf(
                                        "Assets/lorem-ipsum.txt",
                                        this.GetType().Assembly
                                    )
                                )
                            )
                        )
                    )
                ).Value()
            )
            {
                Assert.Equal(
                    3499,
                    new TextBody.Of(
                        new AspNetCoreWire(
                            new AspNetCoreClients()
                        ).Response(
                            new Get($"http://localhost:{port}/")
                        )
                    ).AsString().Replace("\r", "").Replace("\n", "").Length
                );
            }
        }
    }
}
