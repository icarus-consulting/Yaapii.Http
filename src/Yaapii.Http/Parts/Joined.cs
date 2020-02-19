using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Several parts joined together.
    /// </summary>
    public sealed class Joined : MapInput.Envelope
    {
        /// <summary>
        /// Several parts joined together.
        /// </summary>
        public Joined(params IMapInput[] parts) : this(new Many.Of<IMapInput>(parts))
        { }

        /// <summary>
        /// Several parts joined together.
        /// </summary>
        public Joined(IEnumerable<IMapInput> parts) : base(input =>
        {
            foreach (var part in parts)
            {
                input = part.Apply(input);
            }
            return input;
        })
        { }
    }
}
