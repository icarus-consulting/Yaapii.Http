using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Specifies the method for a request.
    /// </summary>
    public sealed partial class Method : MapInput.Envelope
    {
        private const string KEY = "method";

        /// <summary>
        /// Specifies the method for a request.
        /// </summary>
        public Method(string method) : base(new Kvp.Of(KEY, method))
        { }
    }
}
