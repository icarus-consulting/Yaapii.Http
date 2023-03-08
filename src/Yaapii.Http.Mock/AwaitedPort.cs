//MIT License

//Copyright(c) 2023 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Net.NetworkInformation;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Mock
{
    /// <summary>
    /// Port which is awaited for given max timespan to be freed.
    /// </summary>
    public sealed class AwaitedPort : IScalar<int>
    {
        private readonly IScalar<int> port;
        private readonly TimeSpan timeout;

        /// <summary>
        /// Port which is awaited for 30 seconds max to be freed.
        /// </summary>
        public AwaitedPort(int port) : this(port, new TimeSpan(0, 30, 0))
        { }

        /// <summary>
        /// Port which is awaited for 30 seconds max to be freed.
        /// </summary>
        public AwaitedPort(int port, TimeSpan wait) : this(new ScalarOf<int>(port), wait)
        { }

        /// <summary>
        /// Port which is awaited for 30 seconds max to be freed.
        /// </summary>
        public AwaitedPort(IScalar<int> port) : this(port, new TimeSpan(0, 30, 0))
        { }

        /// <summary>
        /// Port which is awaited for given max timespan to be freed.
        /// </summary>
        public AwaitedPort(IScalar<int> port, TimeSpan wait)
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
                throw new ApplicationException($"Cannot use port {this.port.Value()} because it has not been free for {this.timeout.TotalSeconds} seconds.");
            }
            return this.port.Value();
        }

        private bool Available()
        {
            var isAvailable = true;

            foreach (var listener in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
            {
                if (listener.Port == this.port.Value())
                {
                    isAvailable = false;
                    break;
                }
            }
            return isAvailable;
        }
    }
}
