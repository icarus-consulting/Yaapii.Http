using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds the path of a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class Path : MapInput.Envelope
    {
        private const string KEY = "path";

        /// <summary>
        /// Adds the path of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Path(string path) : base(
            new Kvp.Of(KEY, () =>
            {
                if (!path.StartsWith("/"))
                {
                    path = $"/{path}";
                }
                return path;
            })
        )
        { }
    }
}
