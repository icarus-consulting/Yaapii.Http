using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// A mapped Dictionary that does not enumerate the dicitonary
    /// </summary>
    public sealed class MappedDictionary<Out> : ManyEnvelope<Out>
    {
        /// <summary>
        /// A mapped Dictionary that does not enumerate the dicitonary
        /// </summary>
        /// <param name="func">key, value, out</param>
        /// <param name="origin"></param>
        public MappedDictionary(Func<string, Func<string>, Out> func, IDictionary<string, string> origin) : base(new ScalarOf<IEnumerable<Out>>(
            () =>
            {
                IEnumerable<Out> result = new ManyOf<Out>();
                foreach (var key in origin.Keys)
                {
                    result = new Atoms.Enumerable.Joined<Out>(
                        result,
                        func(key, () => origin[key])
                    );
                }
                return result;
            }),
            false)
        { }
    }
}
