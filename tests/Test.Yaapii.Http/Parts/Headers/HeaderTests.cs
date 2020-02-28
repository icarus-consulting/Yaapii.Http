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
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class HeaderTests
    {
        [Fact]
        public void WritesHeader()
        {
            Assert.Equal(
                "some value",
                new Header("some key", "some value").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:some key"]
            );
        }

        [Theory]
        [InlineData(0, "first value")]
        [InlineData(1, "second value")]
        [InlineData(2, "third value")]
        public void WritesMultipleValues(int index, string expected)
        {
            Assert.Equal(
                expected,
                new Request(
                    new Header("same key", "first value"),
                    new Header("same key", "second value"),
                    new Header("same key", "third value")
                )[$"header:{index}:same key"]
            );
        }
    }
}
