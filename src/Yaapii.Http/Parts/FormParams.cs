using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Parts
{
    /// <summary>
    /// Http form parameters for Content-Type application/x-www-form-urlencoded.
    /// </summary>
    public sealed class FormParams : DictInputEnvelope
    {
        public FormParams(IEnumerable<FormParam> parameters) : base(
            new DictOf(
                parameters
            )
        )
        { }

        public sealed class Of : IText
        {
            private readonly IDict dict;

            public Of(IDict dict)
            {
                this.dict = dict;
            }

            public string AsString()
            {
                return
                    new Joined(
                        "&",
                        new Mapped<IKvp, IText>(
                            input => new TextOf($"{input.Key()}={input.Value()}"),
                            new Mapped<IKvp, IKvp>(
                                kvp => new KvpOf(kvp.Key().Substring(2), kvp.Value()),
                                new Filtered<IKvp>(
                                    input => input.Key().StartsWith("f."),
                                    this.dict.Entries()
                                )
                            )
                        )
                    ).AsString();
            }

            public bool Equals(IText other)
            {
                throw new NotImplementedException();
            }
        }
    }
}
