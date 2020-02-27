using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds the user info part of a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class User : MapInput.Envelope
    {
        private const string KEY = "user";

        /// <summary>
        /// Adds the user info part of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public User(string user) : base(new Kvp.Of(KEY, user))
        { }
    }
}
