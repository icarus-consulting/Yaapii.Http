using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Request;

namespace Yaapii.Http.Wire.Test
{
    public sealed class VerifiedTests
    {
        [Fact]
        public void VerifiesResponse()
        {
            var verified = false;
            new Verified(
                new FkWire(200, "OK",
                    new Map.Of("some header key", "this is the right response")
                ),
                new Verification.Verification(res =>
                {
                    if (new FirstOf<string>(new Header.Of(res, "some header key")).Value() == "this is the right response")
                    {
                        verified = true;
                    }
                })
            ).Response(new Get());
            Assert.True(verified);
        }
    }
}
