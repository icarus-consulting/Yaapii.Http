using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;

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
