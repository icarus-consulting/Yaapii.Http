using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds the fragment part of a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class Fragment : MapInput.Envelope
    {
        private const string KEY = "fragment";

        /// <summary>
        /// Adds the fragment part of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Fragment(string fragment) : base(
            new Kvp.Of(KEY, () =>
            {
                if (fragment.StartsWith("#"))
                {
                    fragment = fragment.Remove(0, 1);
                }
                return fragment;
            })
        )
        { }
    }
}
