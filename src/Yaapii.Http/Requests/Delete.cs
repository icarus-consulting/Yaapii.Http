﻿using System;
using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Requests
{
    /// <summary>
    /// A request with delete method.
    /// </summary>
    public sealed class Delete: Map.Envelope
    {
        /// <summary>
        /// A request with delete method.
        /// </summary>
        public Delete(params IMapInput[] parts) : this(new Many.Of<IMapInput>(parts))
        { }

        /// <summary>
        /// A request with delete method.
        /// </summary>
        public Delete(Uri uri, params IMapInput[] parts) : this(
            new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with delete method.
        /// </summary>
        public Delete(string uri, params IMapInput[] parts) : this(
            new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with delete method.
        /// </summary>
        public Delete(IEnumerable<IMapInput> parts) : base(() => 
            new Map.Of(
                new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                    parts,
                    new Method("delete")
                )
            )
        )
        { }
    }
}
