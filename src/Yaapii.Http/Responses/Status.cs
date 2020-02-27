using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Responses
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
