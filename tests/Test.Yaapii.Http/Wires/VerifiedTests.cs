using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Scalar;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Requests;
using Yaapii.Http.Verifications;

namespace Yaapii.Http.Wires.Test
{
    public sealed class VerifiedTests
    {
        [Fact]
        public void VerifiesResponse()
        {
            var verified = false;
            new Verified(
                new FkWire(200, "OK"),
                new Verification(res =>
                {
                    verified = true;
                })
            ).Response(new Get());
            Assert.True(verified);
        }
    }
}
