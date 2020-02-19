using Xunit;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Request.Test
{
    public sealed class PostTests
    {
        [Fact]
        public void AddsMethod()
        {
            Assert.Equal(
                "post",
                new Method.Of(
                    new Post()
                ).AsString()
            );
        }

        [Fact]
        public void AddsUri()
        {
            Assert.Equal(
                "https://localhost/",
                new Address.Of(
                    new Post("https://localhost/")
                ).Value().AbsoluteUri
            );
        }

        [Fact]
        public void AddsOtherParts()
        {
            Assert.Equal(
                "some token", 
                new FirstOf<string>(
                    new BearerTokenAuth.Of(
                        new Post(
                            new BearerTokenAuth("some token")
                        )
                    )
                ).Value()
            );
        }
    }
}
