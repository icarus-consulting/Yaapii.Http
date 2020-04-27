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
using Yaapii.Atoms.Map;
using Yaapii.Http.Fake;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Responses.Test
{
    public sealed class ResponseTests
    {
        [Fact]
        public void SendsRequest()
        {
            var requestSent = false;
            new Response(
                new FkWire(req =>
                {
                    requestSent = true;
                    return new MapOf(new MapInputOf());
                }),
                new Get()
            ).GetEnumerator();
            Assert.True(requestSent);
        }

        [Fact]
        public void OnlySendsOnce()
        {
            var count = 0;
            var response =
                new Response(
                    new FkWire(req =>
                    {
                        count++;
                        return new MapOf(new MapInputOf());
                    }),
                    new Get()
                );
            response.GetEnumerator();
            response.GetEnumerator();
            Assert.Equal(1, count);
        }

        [Fact]
        public void ReadsResponse()
        {
            Assert.Equal(
                "this is the right response",
                new Reason.Of(
                    new Response(
                        new FkWire(
                            200,
                            "this is the right response"
                        ),
                        new Get()
                    )
                ).AsString()
            );
        }

        [Fact]
        public void VerifiesResponse()
        {
            var verified = false;
            new Response(
                new FkWire(200, "OK"),
                new Verifications.Verification(res =>
                {
                    verified = true;
                }),
                new Get()
            ).GetEnumerator();
            Assert.True(verified);
        }
    }
}
