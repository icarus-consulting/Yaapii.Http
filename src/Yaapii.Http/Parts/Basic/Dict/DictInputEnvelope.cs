using System;
using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// Simplify dictionary building.
    /// </summary>
    public abstract class DictInputEnvelope : IDictInput
    {
        private readonly Func<IDict, IDict> origin;

        public DictInputEnvelope(Func<IDictInput> input) : this(
            dict => new JoinedDict(dict, new DictOf(input()))
        )
        { }

        public DictInputEnvelope(IScalar<IDictInput> input) : this(
            dict => new JoinedDict(dict, new DictOf(input.Value()))
        )
        { }

        public DictInputEnvelope(params IKvp[] kvps) : this(
            new EnumerableOf<IKvp>(kvps)
        )
        { }

        public DictInputEnvelope(IEnumerable<IKvp> kvps) : this(
            input => new JoinedDict(input, new HashDict(kvps))
        )
        { }

        public DictInputEnvelope(IDict dict) : this(
            input => {
                return new JoinedDict(input, dict);
            }
        )
        { }

        public DictInputEnvelope(Func<IDict, IDict> origin)
        {
            this.origin = origin;
        }

        public IDict Apply(IDict dict)
        {
            return this.origin.Invoke(dict);
        }

        public IDictInput Self()
        {
            return this;
        }
    }
}
