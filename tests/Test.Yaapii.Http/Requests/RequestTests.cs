using Xunit;
using Yaapii.Http.AtomsTemp.Scalar;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Requests.Test
{
    public sealed class RequestTests
    {
        [Fact]
        public void AddsMethod()
        {
            Assert.Equal(
                "get",
                new Method.Of(
                    new Request("get")
                ).AsString()
            );
        }

        [Fact]
        public void AddsUri()
        {
            Assert.Equal(
                "https://localhost/",
                new Address.Of(
                    new Request("irrelevant", "https://localhost/")
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
                        new Request(
                            new BearerTokenAuth("some token")
                        )
                    )
                ).Value()
            );
        }
    }
}
