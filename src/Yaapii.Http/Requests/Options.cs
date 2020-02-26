﻿using System;
using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Requests
{
    /// <summary>
    /// A request with options method.
    /// </summary>
    public sealed class Options : Map.Envelope
    {
        /// <summary>
        /// A request with options method.
        /// </summary>
        public Options(params IMapInput[] parts) : this(new Many.Of<IMapInput>(parts))
        { }

        /// <summary>
        /// A request with options method.
        /// </summary>
        public Options(Uri uri, params IMapInput[] parts) : this(
            new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with options method.
        /// </summary>
        public Options(string uri, params IMapInput[] parts) : this(
            new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with options method.
        /// </summary>
        public Options(IEnumerable<IMapInput> parts) : base(() => 
            new Map.Of(
                new Yaapii.Atoms.Enumerable.Joined<IMapInput>(
                    parts,
                    new Method("options")
                )
            )
        )
        { }
    }
}