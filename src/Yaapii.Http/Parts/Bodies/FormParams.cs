using System.Collections.Generic;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds form params to a request.
    /// Sets the content type header to application/x-www-form-urlencoded.
    /// </summary>
    public sealed partial class FormParams : MapInput.Envelope
    {
        private const string KEY_PREFIX = "form:";

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(params string[] pairSequence) : this(new Map.Of(pairSequence))
        { }

        /// <summary>
        /// Adds form params to a request.
        /// Sets the content type header to application/x-www-form-urlencoded.
        /// </summary>
        public FormParams(IDictionary<string, string> formParams) : base(() =>
            new Joined(
                new ContentType("application/x-www-form-urlencoded"),
                new MapInput.Of(
                    new Mapped<KeyValuePair<string, string>, IKvp>(origin =>
                        new Kvp.Of($"{KEY_PREFIX}{origin.Key}", origin.Value),
                        formParams
                    )
                )
            )
        )
        { }
    }
}
