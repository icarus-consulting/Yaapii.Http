using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds the port of a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class Port : MapInput.Envelope
    {
        private const string KEY = "port";

        /// <summary>
        /// Adds the port of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Port(int port) : base(new Kvp.Of(KEY, () => $"{port}"))
        { }
    }
}
