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
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Requests
{
    /// <summary>
    /// A request with additional parts added to it.
    /// </summary>
    public sealed class Refined : Map.Envelope
    {
        /// <summary>
        /// A request with additional parts added to it.
        /// </summary>
        public Refined(IDictionary<string, string> origin, params IMapInput[] additional) : this(origin, new Many.Of<IMapInput>(additional))
        { }

        /// <summary>
        /// A request with additional parts added to it.
        /// </summary>
        public Refined(IDictionary<string, string> origin, IEnumerable<IMapInput> additional) : base(() =>
            new Parts.Joined(additional).Apply(
                origin
            )
        )
        { }
    }
}
