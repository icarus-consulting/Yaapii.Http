using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class TextBody
    {
        /// <summary>
        /// The body of a request or response as <see cref="IText"/>
        /// </summary>
        public sealed class Of : TextEnvelope
        {
            /// <summary>
            /// The body of a request or response as <see cref="IText"/>
            /// </summary>
            public Of(IDictionary<string, string> input) : base(
                new TextOf(
                    new Body.Of(input)
                )
            )
            { }
        }
    }
}
