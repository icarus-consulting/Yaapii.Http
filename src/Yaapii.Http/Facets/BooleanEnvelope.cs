using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// Envelope for something returning a boolean value.
    /// </summary>
    public abstract class BooleanEnvelope : IScalar<bool>
    {
        private readonly IScalar<bool> value;

        /// <summary>
        /// Envelope for something returning a boolean value.
        /// </summary>
        protected BooleanEnvelope(Func<bool> value) : this(new Sticky<bool>(value))
        { }

        /// <summary>
        /// Envelope for something returning a boolean value.
        /// </summary>
        protected BooleanEnvelope(IScalar<bool> value)
        {
            this.value = value;
        }

        public bool Value()
        {
            return this.value.Value();
        }
    }
}
