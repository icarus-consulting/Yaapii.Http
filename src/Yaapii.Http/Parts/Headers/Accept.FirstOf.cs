using System.Collections.Generic;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts.Headers
{
    public sealed partial class Accept
    {
        /// <summary>
        /// First value of any "Accept" header field.
        /// </summary>
        public sealed class FirstOf : TextEnvelope
        {
            /// <summary>
            /// First value of any "Accept" header field.
            /// </summary>
            public FirstOf(IDictionary<string, string> input) : base(() =>
                new FirstOf<string>(
                    new Accept.Of(input)
                ).Value(),
                live: false
            )
            { }
        }
    }
}
