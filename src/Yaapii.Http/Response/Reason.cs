using Yaapii.Atoms;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Response
{
    /// <summary>
    /// Adds a reason phrase to a response.
    /// </summary>
    public sealed partial class Reason : MapInput.Envelope
    {
        private const string KEY = "reason";

        /// <summary>
        /// Adds a reason phrase to a response.
        /// </summary>
        public Reason(string reason) : this( new TextOf(reason))
        { }

        /// <summary>
        /// Adds a reason phrase to a response.
        /// </summary>
        public Reason(IText reason) : base(new Kvp.Of(KEY, () => reason.AsString()))
        { }
    }
}
