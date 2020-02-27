using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Bytes;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a body from <see cref="IBytes"/> to a request.
    /// Bytes will be base 64 encoded.
    /// </summary>
    public sealed partial class BytesBody : MapInput.Envelope
    {
        /// <summary>
        /// Adds a body from the bytes of an <see cref="IInput"/> to a request.
        /// Bytes will be base 64 encoded.
        /// </summary>
        public BytesBody(IInput input) : this(new InputAsBytes(input))
        { }

        /// <summary>
        /// Adds a body from <see cref="IBytes"/> to a request.
        /// Bytes will be base 64 encoded.
        /// </summary>
        public BytesBody(IBytes bytes) : base(() =>
            new Body(
                new TextOf(
                    new BytesBase64(bytes)
                )
            )
        )
        { }
    }
}
