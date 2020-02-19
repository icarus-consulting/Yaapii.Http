using System;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Only adds a part if a given condition applies.
    /// </summary>
    public sealed class Conditional : MapInput.Envelope
    {
        /// <summary>
        /// Only adds a part if a given condition applies.
        /// </summary>
        public Conditional(Func<bool> condition, IMapInput consequence) : base(dict => 
            condition() ? consequence.Apply(dict) : dict
        )
        { }
    }
}
