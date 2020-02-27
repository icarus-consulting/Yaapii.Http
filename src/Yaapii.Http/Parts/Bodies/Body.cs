using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Text;

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
        public Body(IText content) : base(() =>
            new MapInput.Of(
                new Kvp.Of(KEY, () => content.AsString())
            )
        )
        { }
    }
}
