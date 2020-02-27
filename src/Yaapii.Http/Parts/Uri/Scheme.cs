using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds the scheme part of a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class Scheme : MapInput.Envelope
    {
        private const string KEY = "scheme";

        /// <summary>
        /// Adds the scheme part of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Scheme(string scheme) : base(new Kvp.Of(KEY, scheme))
        { }
    }
}
