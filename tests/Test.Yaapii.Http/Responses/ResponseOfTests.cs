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

using Xunit;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Responses.Test
{
    public sealed class ResponseOfTests
    {
        [Fact]
        public void AddsStatus()
        {
            Assert.Equal(
                418,
                new Status.Of(
                    new Response.Of(418, "I'm a teapot")
                ).AsInt()
            );
        }

        [Fact]
        public void AddsReason()
        {
            Assert.Equal(
                "I'm a teapot",
                new Reason.Of(
                    new Response.Of(418, "I'm a teapot")
                ).AsString()
            );
        }

        [Fact]
        public void AddsHeaders()
        {
            Assert.Equal(
                "some value",
                new FirstOf<string>(
                    new Header.Of(
                        new Response.Of(
                            418,
                            "I'm a teapot",
                            new Many.Of<IKvp>(
                                new Kvp.Of("some header key", "some value")
                            )
                        ),
                        "some header key"
                    )
                ).Value()
            );
        }

        [Fact]
        public void AddsBody()
        {
            Assert.Equal(
                "mostly hot water",
                new TextBody.Of(
                    new Response.Of(
                        status: 418,
                        reason: "I'm a teapot",
                        headers: new Many.Of<IKvp>(),
                        body: new TextOf("mostly hot water")
                    )
                ).AsString()
            );
        }
    }
}
