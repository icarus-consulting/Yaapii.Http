using Yaapii.Atoms;
using Yaapii.Atoms.Dict;

namespace Yaapii.Http
{
    /// <summary>
    /// The response to a request.
    /// </summary>
    public interface IResponse2
    {
        /// <summary>
        /// Get the response from the request.
        /// </summary>
        /// <returns></returns>
        IDict Dict();

        /// <summary>
        /// Touch the request without caring about the response.
        /// </summary>
        void Touch();
    }
}
