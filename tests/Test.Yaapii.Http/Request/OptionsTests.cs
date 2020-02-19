using Xunit;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Request.Test
{
    public sealed class OptionsTests
    {
        [Fact]
        public void AddsMethod()
        {
            Assert.Equal(
                "options",
                new Method.Of(
                    new Options()
                ).AsString()
            );
        }

        [Fact]
        public void AddsUri()
        {
            Assert.Equal(
                "https://localhost/",
                new Address.Of(
                    new Options("https://localhost/")
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
                        new Options(
                            new BearerTokenAuth("some token")
                        )
                    )
                ).Value()
            );
        }
    }
}
