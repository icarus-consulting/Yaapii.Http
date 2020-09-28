//MIT License

//Copyright(c) 2020 ICARUS Consulting GmbH

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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yaapii.Http.Mock.Templates
{
    /// <summary>
    /// A conditional wire template. Applies if a given condition is met.
    /// </summary>
    public sealed class Conditional : ITemplate
    {
        private readonly Func<IDictionary<string, string>, bool> condition;
        private readonly IWire wire;

        /// <summary>
        /// A conditional wire template. Applies if a given condition is met.
        /// </summary>
        public Conditional(Func<IDictionary<string, string>, bool> condition, IWire wire)
        {
            this.condition = condition;
            this.wire = wire;
        }

        public bool Applies(IDictionary<string, string> request)
        {
            return this.condition(request);
        }

        public Task<IDictionary<string, string>> Response(IDictionary<string, string> request)
        {
            return this.wire.Response(request);
        }
    }
}
