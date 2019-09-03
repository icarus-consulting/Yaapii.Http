using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Atoms.Dict
{
    /// <summary>
    /// A dictionary that rejects being created twice.
    /// </summary>
    public sealed class StrictOnce : IDict
    {
        private readonly Lazy<IDict> dict;

        /// <summary>
        /// A dictionary that rejects being created twice.
        /// </summary>
        public StrictOnce(Func<IDict> dict)
        {
            this.dict = new Lazy<IDict>(dict);
        }

        public bool Contains(string key)
        {
            VerifyOnce();
            return this.dict.Value.Contains(key);
        }

        public string Content(string key)
        {
            VerifyOnce();
            return this.dict.Value.Content(key);
        }

        public string Content(string key, string def)
        {
            VerifyOnce();
            return this.dict.Value.Content(key, def);
        }

        public IEnumerable<IKvp> Entries()
        {
            VerifyOnce();
            return this.dict.Value.Entries();
        }

        private void VerifyOnce()
        {
            if (this.dict.IsValueCreated)
            {
                throw new InvalidOperationException($"The dictionary is restricted to be created only once, but this is the second attempt to create it. Check the code design.");
            }
        }
    }
}
