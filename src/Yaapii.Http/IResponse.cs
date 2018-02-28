using System.Collections.Generic;
using Yaapii.Atoms;

namespace Yaapii.Http
{
    public interface IResponse
    {
        IRequest Back();
        int Status();
        IText Reason();
        IDictionary<string, IList<string>> Headers();
        IInput Body();
    }
}