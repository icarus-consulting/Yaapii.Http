using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Responses
{
    public sealed partial class Response : Map.Envelope
    {
        /// <summary>
        /// A response from the given parts.
        /// </summary>
        public sealed class Of : Map.Envelope
        {
            /// <summary>
            /// A response with the given status, reason, headers and body.
            /// </summary>
            public Of(int status, string reason, IDictionary<string, string> headers, IText body) : this(
                new Many.Of<IMapInput>(
                    new Status(status),
                    new Reason(reason),
                    new Headers(headers),
                    new Body(body)
                )
            )
            { }

            /// <summary>
            /// A response with the given status, reason and headers.
            /// </summary>
            public Of(int status, string reason, IDictionary<string, string> headers) : this(
                new Many.Of<IMapInput>(
                    new Status(status),
                    new Reason(reason),
                    new Headers(headers)
                )
            )
            { }

            /// <summary>
            /// A response with the given status and reason.
            /// </summary>
            public Of(int status, string reason) : this(
                new Many.Of<IMapInput>(
                    new Status(status),
                    new Reason(reason)
                )
            )
            { }

            /// <summary>
            /// A response from the given parts.
            /// </summary>
            public Of(params IMapInput[] responseParts) : this(new Many.Of<IMapInput>(responseParts))
            { }

            /// <summary>
            /// A response from the given parts.
            /// </summary>
            public Of(IEnumerable<IMapInput> responseParts) : base(() => new Map.Of(responseParts))
            { }
        }
    }
}
