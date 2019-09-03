using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// Simplification of Kvp-class-building.
    /// </summary>
    public class KvpEnvelope : IKvp
    {
        private readonly IKvp origin;

        public KvpEnvelope(IKvp origin)
        {
            this.origin = origin;
        }

        public string Key()
        {
            return this.origin.Key();
        }

        public string Value()
        {
            return this.origin.Value();
        }
    }

    /// <summary>
    /// Simplification of Kvp-class-building.
    /// </summary>
    public class KvpEnvelope<TValue> : IKvp<TValue>
    {
        private readonly IKvp<TValue> origin;

        public KvpEnvelope(IKvp<TValue> origin)
        {
            this.origin = origin;
        }

        public string Key()
        {
            return this.origin.Key();
        }

        public TValue Value()
        {
            return this.origin.Value();
        }
    }
}
