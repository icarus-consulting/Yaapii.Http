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

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class HeadersTests
    {
        [Theory]
        [InlineData(0, "first key", "first value")]
        [InlineData(1, "second key", "second value")]
        [InlineData(2, "third key", "third value")]
        public void WritesParams(int index, string key, string expected)
        {
            Assert.Equal(
                expected,
                new Headers(
                    new KvpOf("first key", "first value"),
                    new KvpOf("second key", "second value"),
                    new KvpOf("third key", "third value")
                ).Apply(
                    new MapOf(new MapInputOf())
                )[$"header:{index}:{key}"]
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
                new Headers(
                    new KvpOf("same key", "first value"),
                    new KvpOf("same key", "second value"),
                    new KvpOf("same key", "third value")
                ).Apply(
                    new MapOf(new MapInputOf())
                )[$"header:{index}:same key"]
            );
        }
    }
}
