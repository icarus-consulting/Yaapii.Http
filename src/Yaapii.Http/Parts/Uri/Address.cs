using Yaapii.Atoms;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class Address : MapInput.Envelope
    {
        /// <summary>
        /// Adds a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Address(string uri) : this(new Sticky<System.Uri>(new System.Uri(uri)))
        { }

        /// <summary>
        /// Adds a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Address(System.Uri uri) : this(new ScalarOf<System.Uri>(uri))
        { }

        /// <summary>
        /// Adds a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Address(IScalar<System.Uri> uri) : base(() =>
            new Joined(
                new Scheme(uri.Value().Scheme),
                new Conditional(() =>
                    uri.Value().UserInfo != string.Empty,
                    new User(uri.Value().UserInfo)
                ),
                new Host(uri.Value().Host),
                new Conditional(() =>
                    uri.Value().Port != 0,
                    new Port(uri.Value().Port)
                ),
                new Conditional(() =>
                    uri.Value().AbsolutePath != string.Empty,
                    new Path(uri.Value().AbsolutePath)
                ),
                new Conditional(() =>
                    uri.Value().Query != string.Empty,
                    new QueryParams(uri.Value().Query)
                ),
                new Conditional(() =>
                    uri.Value().Fragment != string.Empty,
                    new Fragment(uri.Value().Fragment)
                )
            )
        )
        { }
    }
}
