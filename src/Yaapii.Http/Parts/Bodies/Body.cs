using Yaapii.Atoms;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a body to a request.
    /// </summary>
    public sealed partial class Body : MapInput.Envelope
    {
        private const string KEY = "body";

        /// <summary>
        /// Adds a body to a request.
        /// </summary>
        public Body(string content) : this(new TextOf(content))
        { }

        /// <summary>
        /// Adds a body to a request.
        /// </summary>
        public Body(IInput content) : this(new TextOf(content))
        { }

        /// <summary>
        /// Adds a body to a request.
        /// </summary>
        public Body(IText content) : base(() =>
            new MapInput.Of(
                new Kvp.Of(KEY, () => content.AsString())
            )
        )
        { }
    }
}
