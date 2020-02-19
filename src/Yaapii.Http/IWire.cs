using System.Collections.Generic;

namespace Yaapii.Http
{
    /// <summary>
    /// Converts a request to a response.
    /// </summary>
    public interface IWire
    {
        /// <summary>
        /// Sends the request, returns the response.
        /// </summary>param>
        IDictionary<string, string> Response(IDictionary<string, string> request);
    }
}
