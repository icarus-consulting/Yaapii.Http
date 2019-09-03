using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;

namespace Yaapii.Http
{
    /// <summary>
    /// Dictionary Verification.
    /// </summary>
    public interface IVerification
    {
        void Verify(IDict dict);
    }
}
