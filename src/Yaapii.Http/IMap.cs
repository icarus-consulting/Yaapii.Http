using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Http
{
    public interface IMap<TKey, TValue> : IDictionary<TKey, TValue>
    { }
}
