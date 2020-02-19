using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Envelope for extracting header values from a request.
    /// </summary>
    public abstract class HeaderOfEnvelope : Many.Envelope<string>
    {
        /// <summary>
        /// Envelope for extracting header values from a request.
        /// </summary>
        protected HeaderOfEnvelope(IDictionary<string, string> input, string key) : base(() => 
            new Mapped<IKvp, string>(matchingHeader =>
                matchingHeader.Value(),
                new Filtered<IKvp>(header =>
                    header.Key() == key,
                    new Headers.Of(input)
                )
            )
        )
        { }
    }
}
