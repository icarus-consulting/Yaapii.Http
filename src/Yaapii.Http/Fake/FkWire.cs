using System;
using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Parts.Body;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Response;

namespace Yaapii.Http.Fake
{
    /// <summary>
    /// A fake wire to convert http requests into responses.
    /// </summary>
    public sealed class FkWire : Wire.Envelope
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
                new Body( body)
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
