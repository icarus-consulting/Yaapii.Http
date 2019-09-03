using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Authorization header.
    /// </summary>
    public sealed class Authorization : DictInputEnvelope
    {
        /// <summary>
        /// Authorization header.
        /// </summary>
        public Authorization(string value) : this(new TextOf(value))
        { }

        /// <summary>
        /// Authorization header.
        /// </summary>
        public Authorization(IText value) : base(new ScalarOf<IDictInput>(() =>
                new Header("Authorization", value)
            )
        )
        { }
    }
}
