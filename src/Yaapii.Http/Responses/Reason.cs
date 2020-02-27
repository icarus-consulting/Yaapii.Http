using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Responses
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
