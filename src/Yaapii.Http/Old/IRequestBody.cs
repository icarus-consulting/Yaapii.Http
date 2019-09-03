using System.Collections;
using System.Collections.Generic;
using Yaapii.Atoms;

namespace Yaapii.Http
{
    public interface IRequestBody
    {
        IRequest Back();
        IText Content();
        IRequestBody With(IText body);

        IRequestBody FormParam(string name, string value);
        IRequestBody FormParam(IDictionary<string, string> param);
    }
}