using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

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
                    new Map.Of(
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
                    new Map.Of(
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
                    new Map.Of(
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
                    new Map.Of(
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
                    new Map.Of(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                ).AsString()
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
                    new Map.Of(
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
                    new Map.Of(
                        new Address("https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment")
                    )
                ).AsString()
            );
        }
    }
}
