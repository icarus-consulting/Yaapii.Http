using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds the host part of a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class Host : MapInput.Envelope
    {
        private const string KEY = "host";

        /// <summary>
        /// Adds the host part of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Host(string host) : base(new Kvp.Of(KEY, host))
        { }
    }
}
