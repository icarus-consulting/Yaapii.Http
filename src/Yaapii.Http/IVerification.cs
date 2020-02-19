using System.Collections.Generic;

namespace Yaapii.Http
{
    /// <summary>
    /// Verifies a request or response.
    /// </summary>
    public interface IVerification
    {
        /// <summary>
        /// Runs the verification.
        /// </summary>
        void Verify(IDictionary<string, string> input);
    }
}
