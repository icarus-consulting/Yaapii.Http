using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Content-Type header.
    /// </summary>
    public sealed class Cookie : DictInputEnvelope
    {
        /// <summary>
        /// Content-Type header.
        /// </summary>
        public Cookie(string value) : base(
            () => new Header("Cookie", value)
        )
        { }
    }
}
