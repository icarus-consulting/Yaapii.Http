using System;
using System.Net.NetworkInformation;
using Yaapii.Atoms;

namespace Yaapii.Http.Mock
{
    /// <summary>
    /// Port which is awaited for given max timespan to be freed.
    /// </summary>
    public sealed class AwaitedPort : IScalar<int>
    {
        private readonly int port;
        private readonly TimeSpan timeout;

        /// <summary>
        /// Port which is awaited for 30 seconds max to be freed.
        /// </summary>
        public AwaitedPort(int port) : this(port, new TimeSpan(0, 30, 0))
        { }

        /// <summary>
        /// Port which is awaited for given max timespan to be freed.
        /// </summary>
        public AwaitedPort(int port, TimeSpan wait)
        {
            this.port = port;
            this.timeout = wait;
        }

        public int Value()
        {
            var start = DateTime.Now;
            while (!Available() && DateTime.Now < start + this.timeout)
            {
                System.Threading.Thread.Sleep(250);
            }
            if (!Available())
            {
                throw new ApplicationException($"Cannot use port {port} because it has not been free for {this.timeout.TotalSeconds} seconds.");
            }
            return port;
        }

        private bool Available()
        {
            var isAvailable = true;

            foreach (var listener in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
            {
                if (listener.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }
            return isAvailable;
        }
    }
}
