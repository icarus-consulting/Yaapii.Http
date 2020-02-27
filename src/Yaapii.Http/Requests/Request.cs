using System;
using System.Collections.Generic;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Requests
{
    /// <summary>
    /// A request from the given parts.
    /// </summary>
    public sealed class Request : Map.Envelope
    {
        /// <summary>
        /// A request from the given parts.
        /// </summary>
        public Request(params IMapInput[] parts) : this(new Many.Of<IMapInput>(parts))
        { }

        /// <summary>
        /// A request with the specified method and the given parts.
        /// </summary>
        public Request(string method, params IMapInput[] parts) : this(
            new Yaapii.Http.AtomsTemp.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Method(method)
            )
        )
        { }

        /// <summary>
        /// A request with the specified <see cref="System.Uri"/> and the given parts.
        /// </summary>
        public Request(Uri uri, params IMapInput[] parts) : this(
            new Yaapii.Http.AtomsTemp.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with the specified method, <see cref="System.Uri"/> and the given parts.
        /// </summary>
        public Request(string method, Uri uri, params IMapInput[] parts) : this(
            new Yaapii.Http.AtomsTemp.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Method(method),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with the specified method, <see cref="System.Uri"/> and the given parts.
        /// </summary>
        public Request(string method, string uri, params IMapInput[] parts) : this(
            new Yaapii.Http.AtomsTemp.Enumerable.Joined<IMapInput>(
                new Many.Of<IMapInput>(parts),
                new Method(method),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request from the given parts.
        /// </summary>
        public Request(IEnumerable<IMapInput> parts) : base(() => new Map.Of(parts))
        { }
    }
}
