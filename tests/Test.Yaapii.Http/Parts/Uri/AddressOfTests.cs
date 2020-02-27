using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class AddressOfTests
    {
        [Fact]
        public void ReadsFullUri()
        {
            Assert.Equal(
                "https://someUser@somehost:1337/this/is/a/path?someQuery=yes&moreQuery=moreYes#someFragment",
                new Address.Of(
                    new Map.Of(
                        new Scheme("https"),
                        new User("someUser"),
                        new Host("somehost"),
                        new Port(1337),
                        new Path("this/is/a/path"),
                        new QueryParam("someQuery", "yes"),
                        new QueryParam("moreQuery", "moreYes"),
                        new Fragment("someFragment")
                    )
                ).Value().ToString()
            );
        }

        [Fact]
        public void ReadsSimpleUri()
        {
            Assert.Equal(
                "https://somehost/",
                new Address.Of(
                    new Map.Of(
                        new Scheme("https"),
                        new Host("somehost")
                    )
                ).Value().ToString()
            );
        }
    }
}
