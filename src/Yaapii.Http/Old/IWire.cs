using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;

namespace Yaapii.Http
{
    public interface IWire
    {
        IResponse
            Send(
                IRequest req, 
                Uri address, 
                IMethod method,
                IEnumerable<KeyValuePair<string, string>> headers, 
                IInput content, 
                TimeSpan timeout
            );
    }
}
