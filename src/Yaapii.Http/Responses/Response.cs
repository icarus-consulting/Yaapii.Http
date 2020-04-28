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
using Yaapii.Atoms.Map;
using Yaapii.Http.Wires;

namespace Yaapii.Http.Responses
{
    /// <summary>
    /// Response from a wire.
    /// </summary>
    public sealed partial class Response : MapEnvelope
    {
        /// <summary>
        /// Verified response from a wire.
        /// </summary>
        public Response(IWire wire, IVerification verification) : this(new Verified(wire, verification))
        { }

        /// <summary>
        /// Response from a wire.
        /// </summary>
        public Response(IWire wire) : this(wire, new MapOf(new MapInputOf()))
        { }

        /// <summary>
        /// Verified response from a wire.
        /// </summary>
        public Response(IWire wire, IVerification verification, IDictionary<string, string> request) : this(new Verified(wire, verification), request)
        { }

        /// <summary>
        /// Response from a wire.
        /// </summary>
        public Response(IWire wire, IDictionary<string, string> request) : base(() =>
            wire.Response(request),
            live: false
        )
        { }
    }
}
