using System;
using System.Net;
using Xunit;

namespace Yaapii.Http.Wires.AspNetCore.Test
{
    public sealed class AspNetCoreClientsTests
    {
        [Fact]
        public void ReusesClients()
        {
            var clients = new AspNetCoreClients();
            var first =
                clients.Client(
                    new TimeSpan(0, 1, 0)
                );
            first.BaseAddress = new Uri("http://localhost/this/is/a/test");
            var second =
                clients.Client(
                    new TimeSpan(0, 1, 0) // same timeout should return same client
                );
            Assert.Equal(
                first.BaseAddress,
                second.BaseAddress
            );
        }

        [Fact]
        public void ReturnsDifferentClients()
        {
            var clients = new AspNetCoreClients();
            var first =
                clients.Client(
                    new TimeSpan(0, 1, 1)
                );
                first.BaseAddress = new Uri("http://localhost/this/is/a/test");
            var second =
                clients.Client(
                    new TimeSpan(0, 2, 1) // different timeout should return new client
                );
            Assert.NotEqual(
                first.BaseAddress,
                second.BaseAddress
            );
        }

        [Fact]
        public void SetsDefaultSecurityTypes()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            new AspNetCoreClients().Client(new TimeSpan(0, 1, 0));
            Assert.Equal(
                ServicePointManager.SecurityProtocol,
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
            );
        }
    }
}
