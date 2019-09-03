using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Basic authorization header.
    /// </summary>
    public sealed class BasicAuth : DictInputEnvelope
    {
        /// <summary>
        /// Basic authorization header.
        /// </summary>
        public BasicAuth(string username, string password) : base(
            new ScalarOf<IDictInput>(() =>
                new Authorization(
                    new Formatted(
                        "Basic {0}",
                        new TextBase64(
                            new Formatted("{0}:{1}", username, password)
                        )
                    )
                ).Self()
            )
        )
        { }
    }
}
