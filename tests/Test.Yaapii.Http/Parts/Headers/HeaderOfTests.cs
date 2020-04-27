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
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class HeaderOfTests
    {
        [Fact]
        public void ReadsHeader()
        {
            Assert.Equal(
                "some value",
                new FirstOf<string>(
                    new Header.Of(
                        new MapOf("header:0:some key", "some value"),
                        "some key"
                    )
                ).Value()
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
                new ItemAt<string>(
                    new Header.Of(
                        new MapOf(
                            "header:0:same key", "first value",
                            "header:1:same key", "second value",
                            "header:2:same key", "third value"
                        ),
                        "same key"
                    ),
                    index
                ).Value()
            );
        }
    }
}
