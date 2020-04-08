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
using Yaapii.Atoms;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Responses;

namespace Yaapii.Http.Fake
{
    /// <summary>
    /// A fake wire to convert http requests into responses.
    /// </summary>
    public sealed class FkWire : Wires.WireEnvelope
    {
        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Always returns status code 200 and no body or headers.
        /// </summary>
        public FkWire() : this(200, "OK", new Map.Of(new MapInput.Of()))
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Always returns status code 200 and no body.
        /// </summary>
        public FkWire(IDictionary<string, string> headers) : this(200, "OK", headers)
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Always returns status code 200 and no headers.
        /// </summary>
        public FkWire(IText body) : this(200, "OK", new Map.Of(new MapInput.Of()), body)
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Always returns status code 200.
        /// </summary>
        public FkWire(IDictionary<string, string> headers, IText body) : this(200, "OK", headers, body)
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code, reason, headers and body.
        /// </summary>
        public FkWire(int status, string reason, IDictionary<string, string> headers, IText body) : this(
            new Many.Of<IMapInput>(
                new Status(status),
                new Reason(reason),
                new Headers(headers),
                new Body(body)
            )
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code, reason and headers.
        /// </summary>
        public FkWire(int status, string reason, IDictionary<string, string> headers) : this(
            new Many.Of<IMapInput>(
                new Status(status),
                new Reason(reason),
                new Headers(headers)
            )
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code and reason.
        /// </summary>
        public FkWire(int status, string reason) : this(
            new Many.Of<IMapInput>(
                new Status(status),
                new Reason(reason)
            )
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Creates a response from the given parts.
        /// </summary>
        public FkWire(IMapInput[] responseParts) : this(new Many.Of<IMapInput>(responseParts))
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Creates a response from the given parts.
        /// </summary>
        public FkWire(IEnumerable<IMapInput> responseParts) : this(req => new Map.Of(responseParts))
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Creates a response using the given function.
        /// </summary>
        public FkWire(Func<IDictionary<string, string>, IDictionary<string, string>> response) : base(response)
        { }
    }
}
