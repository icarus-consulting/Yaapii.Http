using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Atoms
{
    /// <summary>
    /// Input to be attached to a dictionary.
    /// </summary>
    public interface IDictInput
    {
        /// <summary>
        /// Apply the input to a dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns>Updated dictionary</returns>
        IDict Apply(IDict dict);

        /// <summary>
        /// Access the raw IDictInput.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns>Itself as IDictInput.</returns>
        IDictInput Self();
    }

    /// <summary>
    /// Input to be attached to a dictionary.
    /// </summary>
    public interface IDictInput<TValue>
    {
        /// <summary>
        /// Apply the input to a dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns>Updated dictionary</returns>
        IDict<TValue> Apply(IDict<TValue> dict);

        /// <summary>
        /// Access the raw IDictInput.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns>Itself as IDictInput.</returns>
        IDictInput<TValue> Self();
    }
}
