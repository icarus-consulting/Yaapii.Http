﻿//MIT License

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

using Yaapii.Http.Facets;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Wires;

namespace Yaapii.Http.Responses
{
    /// <summary>
    /// JSON data received as a response from the given wire.
    /// </summary>
    public sealed partial class JsonResponse : JsonEnvelope
    {
        /// <summary>
        /// JSON data received as a response from the given wire.
        /// </summary>
        public JsonResponse(IWire wire, IVerification verification) : this(
            new Verified(wire, verification)
        )
        { }

        /// <summary>
        /// JSON data received as a response from the given wire.
        /// </summary>
        public JsonResponse(IWire wire) : this(
            wire,
            new SimpleMessage()
        )
        { }

        /// <summary>
        /// JSON data received as a response from the given wire.
        /// </summary>
        public JsonResponse(IWire wire, IVerification verification, IMessage request) : this(
            new Verified(wire, verification),
            request
        )
        { }

        /// <summary>
        /// JSON data received as a response from the given wire.
        /// </summary>
        public JsonResponse(IWire wire, IMessage request) : base(() =>
            new JsonBody.Of(
                wire.Response(request)
            )
        )
        { }
    }
}
