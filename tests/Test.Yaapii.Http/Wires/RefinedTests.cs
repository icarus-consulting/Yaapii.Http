using Xunit;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Wires.Test
{
    public sealed class RefinedTests
    {
        [Fact]
        public void AddsRequestParts()
        {
            var token = "";
            new Refined(
                new FkWire(req =>
                {
                    token = new FirstOf<string>(new BearerTokenAuth.Of(req)).Value();
                    return new Responses.Response.Of(200, "OK");
                }),
                new BearerTokenAuth("this is a token")
            ).Response(new Get());
            Assert.Equal(
                "this is a token",
                token
            );
        }
    }
}
