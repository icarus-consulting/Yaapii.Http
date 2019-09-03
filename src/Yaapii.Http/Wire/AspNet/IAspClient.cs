using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Yaapii.Atoms;

namespace Yaapii.Http.Wire.AspNet
{
    /// <summary>
    /// Asp Client
    /// </summary>
    public interface IAspClient
    {
        HttpResponseMessage Response(IDict request);
    }
}
