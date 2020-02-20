﻿using System;
using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Requests
{
    /// <summary>
    /// A request with head method.
    /// </summary>
    public sealed class Head : Map.Envelope
    {
        /// <summary>
        /// A request with head method.
        /// </summary>
        public Head(params IMapInput[] parts) : this(new Many.Of<IMapInput>(parts))
        { }

        /// <summary>
        /// A request with head method.
        /// </summary>
        public Head(Uri uri, params IMapInput[] parts) : this(
            new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with head method.
        /// </summary>
        public Head(string uri, params IMapInput[] parts) : this(
            new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with head method.
        /// </summary>
        public Head(IEnumerable<IMapInput> parts) : base(() => 
            new Map.Of(
                new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                    parts,
                    new Method("head")
                )
            )
        )
        { }
    }
}
