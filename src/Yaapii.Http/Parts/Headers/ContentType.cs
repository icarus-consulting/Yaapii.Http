using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Content-Type header.
    /// </summary>
    public sealed class ContentType : DictInputEnvelope
    {
        /// <summary>
        /// Content-Type header.
        /// </summary>
        public ContentType(string value) : base(
            () => new Header("Content-Type", value)
        )
        { }
    }
}
