using System;
using System.Collections.Generic;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Fake
{
    /// <summary>
    /// A fake part to build a http request or response from.
    /// </summary>
    public sealed class FkPart : MapInput.Envelope
    {
        /// <summary>
        /// A fake part to build a http request or response from.
        /// </summary>
        public FkPart(Func<IMapInput> input) : base(input)
        { }

        /// <summary>
        /// A fake part to build a http request or response from.
        /// </summary>
        public FkPart(Func<IDictionary<string, string>, IDictionary<string, string>> apply) : base(apply)
        { }
    }
}
