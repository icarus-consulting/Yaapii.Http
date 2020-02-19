using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Response
{
    /// <summary>
    /// Adds a status code to a response.
    /// </summary>
    public sealed partial class Status : MapInput.Envelope
    {
        private const string KEY = "status";

        /// <summary>
        /// Adds a status code to a response.
        /// </summary>
        public Status(int status) : base(new Kvp.Of(KEY, () => $"{status}"))
        { }
    }
}
