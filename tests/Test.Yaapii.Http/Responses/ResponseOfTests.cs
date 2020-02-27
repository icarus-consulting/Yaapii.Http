using Xunit;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Scalar;
using Yaapii.Http.AtomsTemp.Text;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Responses.Test
{
    public sealed class ResponseOfTests
    {
        [Fact]
        public void AddsStatus()
        {
            Assert.Equal(
                418,
                new Status.Of(
                    new Response.Of(418, "I'm a teapot")
                ).AsInt()
            );
        }

        [Fact]
        public void AddsReason()
        {
            Assert.Equal(
                "I'm a teapot",
                new Reason.Of(
                    new Response.Of(418, "I'm a teapot")
                ).AsString()
            );
        }

        [Fact]
        public void AddsHeaders()
        {
            Assert.Equal(
                "some value",
                new FirstOf<string>(
                    new Header.Of(
                        new Response.Of(
                            418,
                            "I'm a teapot",
                            new Many.Of<IKvp>(
                                new Kvp.Of("some header key", "some value")
                            )
                        ),
                        "some header key"
                    )
                ).Value()
            );
        }

        [Fact]
        public void AddsBody()
        {
            Assert.Equal(
                "mostly hot water",
                new Body.Of(
                    new Response.Of(
                        status: 418,
                        reason: "I'm a teapot",
                        headers: new Many.Of<IKvp>(),
                        body: new TextOf("mostly hot water")
                    )
                ).AsString()
            );
        }
    }
}
