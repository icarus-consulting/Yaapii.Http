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

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class AddressTests
    {
        [Fact]
        public void WritesScheme()
        {
            Assert.Equal(
                "https",
                new Scheme.Of(
                    new SimpleMessage(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                ).AsString()
            );
        }

        [Fact]
        public void WritesUser()
        {
            Assert.Equal(
                "someUser",
                new User.Of(
                    new SimpleMessage(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                ).AsString()
            );
        }

        [Fact]
        public void WritesHost()
        {
            Assert.Equal(
                "somehost",
                new Host.Of(
                    new SimpleMessage(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                ).AsString()
            );
        }

        [Fact]
        public void WritesPort()
        {
            Assert.Equal(
                1337,
                new Port.Of(
                    new SimpleMessage(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                ).AsInt()
            );
        }

        [Fact]
        public void WritesPath()
        {
            Assert.Equal(
                "/this/is/a/path",
                new Path.Of(
                    new SimpleMessage(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                ).AsString()
            );
        }

        [Fact]
        public void DoesNotWriteEmptyPathFromSystemUri()
        {
            Assert.False(
                new Path.Exists(
                    new SimpleMessage(
                        new Address(
                            new System.Uri("https://someUser@somehost:1337/?someQuery=yes&moreQuery=moreYes#someFragment")
                        )
                    )
                ).Value()
            );
        }

        [Theory]
        [InlineData("someQuery", "yes")]
        [InlineData("moreQuery", "moreYes")]
        public void WritesQuery(string key, string expected)
        {
            Assert.Equal(
                expected,
                new QueryParams.Of(
                    new SimpleMessage(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                )[key]
            );
        }

        [Fact]
        public void WritesFragment()
        {
            Assert.Equal(
                "someFragment",
                new Fragment.Of(
                    new SimpleMessage(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                ).AsString()
            );
        }
    }
}
