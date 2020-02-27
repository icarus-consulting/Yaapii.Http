using System;
using Xunit;
using Yaapii.Http.Fake;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Wires.Test
{
    public sealed class RetryTests
    {
        [Fact]
        public void Retries()
        {
            var attempt = 0;
            var attempts = 3;
            new Retry(
                attempts,
                new FkWire(req =>
                {
                    attempt++;
                    if(attempt < attempts)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        return new Responses.Response.Of(200, "OK");
                    }
                })
            ).Response(new Get());
            Assert.Equal(attempts,  attempt);
        }

        [Fact]
        public void ThrowsAfterLastAttempt()
        {
            var attempt = 0;
            var attempts = 3;
            Assert.Throws<ApplicationException>(() =>
                new Retry(
                    attempts,
                    new FkWire(req =>
                    {
                        attempt++;
                        throw new Exception();
                    })
                ).Response(new Get("http://localhost"))
            );
            Assert.Equal(attempts, attempt);
        }
    }
}
