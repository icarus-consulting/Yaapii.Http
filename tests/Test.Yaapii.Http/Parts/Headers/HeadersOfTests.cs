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

using Xunit;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class HeadersOfTests
    {
        [Theory]
        [InlineData("first key", "first value")]
        [InlineData("second key", "second value")]
        [InlineData("third key", "third value")]
        public void ReadsParams(string key, string expected)
        {
            Assert.Equal(
                expected,
                new FirstOf<IKvp>(kvp =>
                    kvp.Key() == key,
                    new Headers.Of(
                        new SimpleMessage(
                            new MapOf(
                                "header:0:first key", "first value",
                                "header:1:second key", "second value",
                                "header:2:third key", "third value"
                            )
                        )
                    )
                ).Value().Value()
            );
        }
        
        [Theory]
        [InlineData(0, "first value")]
        [InlineData(1, "second value")]
        [InlineData(2, "third value")]
        public void ReadsMultipleValues(int index, string expected)
        {
            Assert.Equal(
                expected,
                new ItemAt<IKvp>(
                    new Atoms.Enumerable.Filtered<IKvp>(kvp =>
                        kvp.Key() == "same key",
                        new Headers.Of(
                            new SimpleMessage(
                                new MapOf(
                                    "header:0:same key", "first value",
                                    "header:1:same key", "second value",
                                    "header:2:same key", "third value"
                                )
                            )
                        )
                    ),
                    index
                ).Value().Value()
            );
        }
    }
}
