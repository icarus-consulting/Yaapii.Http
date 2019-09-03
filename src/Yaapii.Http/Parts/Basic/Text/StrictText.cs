using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Yaapii.Machine.IO.Parts.Basic
{
    /// <summary>
    /// A strict text which can only be one of the specified valid texts.
    /// </summary>
    public sealed class StrictText : IText
    {
        private readonly IText candidate;
        private readonly IEnumerable<IText> valids;

        /// <summary>
        /// A strict text which can only be one of the specified valid texts.
        /// </summary>
        public StrictText(string candidate, params string[] valid) : this(new TextOf(candidate), valid)
        { }

        /// <summary>
        /// A strict text which can only be one of the specified valid texts.
        /// </summary>
        public StrictText(IText candidate, params string[] valid) : this(
            candidate,
            new Many<IText>(
                new Mapped<string, IText>(
                    str => new TextOf(str), valid)
                )
            )
        { }

        /// <summary>
        /// A strict text which can only be one of the specified valid texts.
        /// </summary>
        public StrictText(IText candidate, params IText[] valid) : this(candidate, new Many<IText>(valid))
        { }

        /// <summary>
        /// A strict text which can only be one of the specified valid texts.
        /// </summary>
        public StrictText(IText candidate, IEnumerable<IText> valids)
        {
            this.candidate = candidate;
            this.valids = valids;
        }

        public string AsString()
        {
            var strCandidate = this.candidate.AsString();
            foreach (var valid in this.valids)
            {
                if (valid.AsString().Equals(strCandidate))
                {
                    return strCandidate;
                }
            }
            throw new NotImplementedException($"'{strCandidate}' is not valid. Valids are: "
                + string.Join(", ", new Mapped<IText, string>(
                    vld => vld.AsString(), this.valids
                ))
            );
        }

        public bool Equals(IText other)
        {
            return this.AsString().Equals(other.AsString());
        }
    }
}
