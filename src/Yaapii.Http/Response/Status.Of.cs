using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Number;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Response
{
    public sealed partial class Status : MapInput.Envelope
    {
        /// <summary>
        /// Gets the status code of a response.
        /// </summary>
        public sealed class Of : NumberEnvelope
        {
            /// <summary>
            /// Gets the status code of a response.
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
