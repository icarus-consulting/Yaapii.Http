using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Atoms
{
    /// <summary>
    /// A key-value pair.
    /// </summary>
    public interface IKvp
    {
        string Key();
        string Value();
    }

    /// <summary>
    /// A key-value pair.
    /// </summary>
    public interface IKvp<TValue>
    {
        string Key();
        TValue Value();
    }
}
