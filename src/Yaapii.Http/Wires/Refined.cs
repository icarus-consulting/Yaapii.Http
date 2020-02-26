using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Http.Wires
{
    /// <summary>
    /// A wire that adds extra parts to every request.
    /// </summary>
    public sealed class Refined : WireEnvelope
    {
        /// <summary>
        /// A wire that adds extra parts to every request.
        /// </summary>
        public Refined(IWire origin, params IMapInput[] requestParts) : this(origin, new Many.Of<IMapInput>(requestParts))
        { }

        /// <summary>
        /// A wire that adds extra parts to every request.
        /// </summary>
        public Refined(IWire origin, IEnumerable<IMapInput> requestParts) : base(request =>
            origin.Response(
                new Requests.Refined(
                    request,
                    requestParts
                )
            )
        )
        { }
    }
}
