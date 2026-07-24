using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using System.Linq;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Test
{
    /// <summary>
    /// Port of a started web host.
    /// </summary>
    public sealed class RunningPort : ScalarEnvelope<int>
    {
        /// <summary>
        /// Port of a started web host.
        /// </summary>
        public RunningPort(IWebHost host) : base(() =>
        {
            var addresses = host.ServerFeatures.Get<IServerAddressesFeature>();
            if (addresses == null || addresses.Addresses.Count == 0)
            {
                throw new InvalidOperationException("Cannot detect port of server because it has no server addresses.");
            }
            return new Uri(addresses.Addresses.First()).Port;
        })
        { }
    }
}
