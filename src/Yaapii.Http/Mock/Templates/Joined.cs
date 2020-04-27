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

using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Http.Mock.Templates
{
    /// <summary>
    /// Several templates joined together. 
    /// Matches if all given templates match. Returns response of the specified primary template.
    /// </summary>
    public sealed class Joined : ITemplate
    {
        private readonly ITemplate primary;
        private readonly IEnumerable<ITemplate> extraConditions;

        /// <summary>
        /// Several templates joined together. 
        /// Matches if all given templates match. Returns response of the specified primary template.
        /// </summary>
        public Joined(ITemplate primary, params ITemplate[] extraConditions): this(primary, new ManyOf<ITemplate>(extraConditions))
        { }

        /// <summary>
        /// Several templates joined together. 
        /// Matches if all given templates match. Returns response of the specified primary template.
        /// </summary>
        public Joined(ITemplate primary, IEnumerable<ITemplate> extraConditions)
        {
            this.primary = primary;
            this.extraConditions = extraConditions;
        }

        public bool Applies(IDictionary<string, string> request)
        {
            var applies = this.primary.Applies(request);
            foreach(var condition in this.extraConditions)
            {
                if(!applies)
                {
                    break;
                }
                applies &= condition.Applies(request);
            }
            return applies;
        }

        public IDictionary<string, string> Response(IDictionary<string, string> request)
        {
            return this.primary.Response(request);
        }
    }
}
