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

namespace Yaapii.Http.Mock
{
    /// <summary>
    /// First port in a given range which is pre-tested to be free.
    /// </summary>
    public sealed class RandomPort : IScalar<int>
    {
        private readonly int min;
        private readonly int max;
        private const int TRIES = 1024;

        /// <summary>
        /// Port which is tested to be free.
        /// </summary>
        public RandomPort() : this(1000, 65535)
        { }

        /// <summary>
        /// First port in a given range which is pre-tested to be free.
        /// </summary>
        public RandomPort(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public int Value()
        {
            int result = -1;
            int currentTry = 0;


            if (min > max)
            {
                throw new ArgumentException($"Minimum port must be lower or equal to maximum port, but it isn't: {min} > {max}");
            }

            if (min < 1 || min > 65535 || max < 0 || max > 65535)
            {
                throw new ArgumentException($"A random port must be in range 1 to 65535");
            }
            while (result == -1 && currentTry < TRIES)
            {
                currentTry++;
                result = new Random(Guid.NewGuid().GetHashCode()).Next(this.min, this.max);
                var listeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();

                foreach (var tcpi in listeners)
                {
                    if (tcpi.Port == result)
                    {
                        result = -1;
                        break;
                    }
                }
                
            }

            if (result == -1)
            {
                throw new ArgumentException($"No free random port could be found in the range {this.min} - {this.max} within {TRIES} tries");
            }
            return result;
        }
}
}
