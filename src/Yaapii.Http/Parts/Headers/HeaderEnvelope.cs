using System;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Envelope for adding a header field to a request.
    /// </summary>
    public abstract class HeaderEnvelope : MapInput.Envelope
    {
        /// <summary>
        /// Envelope for adding a header field to a request.
        /// </summary>
        protected HeaderEnvelope(string key, string value) : this(() => key, () => value)
        { }

        /// <summary>
        /// Envelope for adding a header field to a request.
        /// </summary>
        protected HeaderEnvelope(Func<string> key, Func<string> value) : base(input =>
            new Headers(
                new Kvp.Of(key(), value())
            ).Apply(input)
        )
        { }
    }
}
