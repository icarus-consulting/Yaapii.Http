using System.Collections.Generic;
using Yaapii.Atoms.Number;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts.Uri
{
    public sealed partial class Port
    {
        /// <summary>
        /// Extracts the port of a <see cref="System.Uri"/> from a request.
        /// </summary>
        public sealed class Of : NumberEnvelope
        {
            /// <summary>
            /// Extracts the port of a <see cref="System.Uri"/> from a request.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(
                new IntOf(
                    new TextOf(() => input[KEY])
                )
            )
            { }
        }
    }
}
