using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Headers;
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
                    if (new FirstOf<string>(new Header.Of(req, "some header key")).Value() == "this is the right request")
                    {
                        requestSent = true;
                    }
                    return new Map.Of(new MapInput.Of());
                }),
                new Get(
                    new Header(
                        "some header key", "this is the right request"
                    )
                )
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
                new FkWire(
                    200,
                    "this is the right response"
                ),
                new Verifications.Verification(res =>
                {
                    if (new Reason.Of(res).AsString() == "this is the right response")
                    {
                        verified = true;
                    }
                }),
                new Get(
                    new Header(
                        "some header key", "this is the right request"
                    )
                )
            ).GetEnumerator();
            Assert.True(verified);
        }
    }
}
