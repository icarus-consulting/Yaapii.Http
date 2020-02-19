using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Wire;

namespace Yaapii.Http.Response
{
    /// <summary>
    /// Response from a wire.
    /// </summary>
    public sealed partial class Response : Map.Envelope
    {
        /// <summary>
        /// Verified response from a wire.
        /// </summary>
        public Response(IWire wire, IVerification verification) : this(new Verified(wire, verification))
        { }

        /// <summary>
        /// Response from a wire.
        /// </summary>
        public Response(IWire wire) : this(wire, new Map.Of(new MapInput.Of()))
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
            wire.Response(request)
        )
        { }
    }
}
