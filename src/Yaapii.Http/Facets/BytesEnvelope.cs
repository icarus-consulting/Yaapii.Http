using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// Envelope for something returning an array of bytes.
    /// </summary>
    public abstract class BytesEnvelope : IBytes
    {
        private readonly IScalar<byte[]> bytes;

        /// <summary>
        /// Envelope for something returning an array of bytes.
        /// </summary>
        protected BytesEnvelope(Func<byte[]> bytes) : this(new ScalarOf<byte[]>(bytes))
        { }

        /// <summary>
        /// Envelope for something returning an array of bytes.
        /// </summary>
        protected BytesEnvelope(IScalar<byte[]> bytes)
        {
            this.bytes = bytes;
        }

        public byte[] AsBytes()
        {
            return this.bytes.Value();
        }
    }
}
