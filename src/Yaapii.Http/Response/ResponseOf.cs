using System;
using Yaapii.Atoms;

namespace Yaapii.Http.Response
{
    /// <summary>
    /// Converts a http request to a response.
    /// </summary>
    public sealed class ResponseOf2 : IResponse2
    {
        public ResponseOf2(IWire wire, IDict request, IVerification verification)
        {
            throw new NotImplementedException();
        }

        public IDict Dict()
        {
            throw new NotImplementedException();
        }

        public void Touch()
        {
            throw new NotImplementedException();
        }
    }
}
