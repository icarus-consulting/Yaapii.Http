using System;
using Xunit;

namespace Yaapii.Http.Wires.AspNetCore.Test
{
    public sealed class AspNetCoreClientTests
    {
        [Fact]
        public void ReusesClients()
        {
            var first =
                new AspNetCoreClient(
                    new TimeSpan(0, 1, 0)
                ).Value();
            first.BaseAddress = new Uri("http://localhost/this/is/a/test");
            var second =
                new AspNetCoreClient(
                    new TimeSpan(0, 1, 0) // same timeout should return same client
                ).Value();
            Assert.Equal(
                first.BaseAddress,
                second.BaseAddress
            );
        }

        [Fact]
        public void ReturnsDifferentClients()
        {
            var first =
                new AspNetCoreClient(
                    new TimeSpan(0, 1, 1)
                ).Value();
                first.BaseAddress = new Uri("http://localhost/this/is/a/test");
            var second =
                new AspNetCoreClient(
                    new TimeSpan(0, 2, 1) // different timeout should return new client
                ).Value();
            Assert.NotEqual(
                first.BaseAddress,
                second.BaseAddress
            );
        }
    }
}
