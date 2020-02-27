using System;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Scalar;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// Envelope for something returning a <see cref="Uri"/>.
    /// </summary>
    public abstract class UriEnvelope : IScalar<System.Uri>
    {
        private readonly IScalar<System.Uri> uri;

        /// <summary>
        /// Envelope for something returning a <see cref="Uri"/>.
        /// </summary>
        protected UriEnvelope(Func<System.Uri> uri) : this(new Sticky<System.Uri>(uri))
        { }

        /// <summary>
        /// Envelope for something returning a <see cref="Uri"/>.
        /// </summary>
        protected UriEnvelope(IScalar<System.Uri> uri)
        {
            this.uri = uri;
        }

        public System.Uri Value()
        {
            return this.uri.Value();
        }
    }
}
