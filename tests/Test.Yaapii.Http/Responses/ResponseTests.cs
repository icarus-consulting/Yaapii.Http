using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Fake;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Responses.Test
{
    public sealed class ResponseTests
    {
        [Fact]
        public void SendsRequest()
        {
            var requestSent = false;
            new Response(
                new FkWire(req =>
                {
                    requestSent = true;
                    return new Map.Of(new MapInput.Of());
                }),
                new Get()
            ).GetEnumerator();
            Assert.True(requestSent);
        }

        [Fact]
        public void OnlySendsOnce()
        {
            var count = 0;
            var response =
                new Response(
                    new FkWire(req =>
                    {
                        count++;
                        return new Map.Of(new MapInput.Of());
                    }),
                    new Get()
                );
            response.GetEnumerator();
            response.GetEnumerator();
            Assert.Equal(1, count);
        }

        [Fact]
        public void ReadsResponse()
        {
            Assert.Equal(
                "this is the right response",
                new Reason.Of(
                    new Response(
                        new FkWire(
                            200,
                            "this is the right response"
                        ),
                        new Get()
                    )
                ).AsString()
            );
        }

        [Fact]
        public void VerifiesResponse()
        {
            var verified = false;
            new Response(
                new FkWire(200, "OK"),
                new Verifications.Verification(res =>
                {
                    verified = true;
                }),
                new Get()
            ).GetEnumerator();
            Assert.True(verified);
        }
    }
}
