using System;
using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Request
{
    /// <summary>
    /// A request with put method.
    /// </summary>
    public sealed class Put: Map.Envelope
    {
        /// <summary>
        /// A request with put method.
        /// </summary>
        public Put(params IMapInput[] parts) : this(new Many.Of<IMapInput>(parts))
        { }

        /// <summary>
        /// A request with put method.
        /// </summary>
        public Put(Uri uri, params IMapInput[] parts) : this(
            new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with put method.
        /// </summary>
        public Put(string uri, params IMapInput[] parts) : this(
            new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with put method.
        /// </summary>
        public Put(IEnumerable<IMapInput> parts) : base(() => 
            new Map.Of(
                new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                    parts,
                    new Method("put")
                )
            )
        )
        { }
    }
}
