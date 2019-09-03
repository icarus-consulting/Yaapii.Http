using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Atoms
{
    /// <summary>
    /// A string dictionary
    /// </summary>
    public interface IDict
    {
        string Content(string key);
        string Content(string key, string def);
        bool Contains(string key);
        IEnumerable<IKvp> Entries();
    }

    /// <summary>
    /// A string-value dictionary
    /// </summary>
    public interface IDict<TValue>
    {
        TValue Content(string key);
        TValue Content(string key, TValue def);
        bool Contains(string key);
        IEnumerable<IKvp<TValue>> Entries();
    }
}
