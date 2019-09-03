using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Yaapii.Http.Wire.AspNet
{
    /// <summary>
    /// Optional context to further configure a Asp Http Client.
    /// </summary>
    public interface IAspContext
    {
        void Apply(HttpClient client);
    }
}
