using System;
using System.Net.Http;

namespace Yaapii.Http.Wires.AspNetCore
{
    /// <summary>
    /// Manages reuseable <see cref="HttpClient"/>s.
    /// See https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/ for why they should be reused.
    /// </summary>
    public interface IAspHttpClients
    {
        /// <summary>
        /// Gets a previously used client or creates a new client with the given timeout.
        /// </summary>
        HttpClient Client(TimeSpan timeout);
    }
}
