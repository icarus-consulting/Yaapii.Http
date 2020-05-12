using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// A filtered Dictionary that does not enumerate the dictionary
    /// </summary>
    public sealed class FilteredDictionary : MapEnvelope
    {

        /// <summary>
        /// A filtered Dictionary that does not enumerate the dictionary
        /// </summary>
        public FilteredDictionary(Func<string, Func<string>, bool> func, IDictionary<string, string> origin) : base(() =>
            {
                IEnumerable<IKvp> result = new ManyOf<IKvp>();
                foreach (var key in origin.Keys)
                {
                    if (func(key, () => origin[key]))
                    {
                        result = new Yaapii.Atoms.Enumerable.Joined<IKvp>(result,
                            new KvpOf(key, () => origin[key])
                        );
                    }
                }
                return new MapOf(result);
            },
            false
            )
        { }
    }
}
