using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Accept header.
    /// </summary>
    public sealed class Accept : DictInputEnvelope
    {
        /// <summary>
        /// Accept header.
        /// </summary>
        public Accept(string value) : base(new ScalarOf<IDictInput>(() =>
                new Header("accept", value)
            )
        )
        { }
    }
}
