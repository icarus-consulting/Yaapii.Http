using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;

namespace Yaapii.Http
{
    public interface IRequestUri
    {
        IRequest Back();
        Uri Value();
        IRequestUri Uri(Uri url);
        IRequestUri Path(string segment);
        IRequestUri Port(int num);
        IRequestUri QueryParam(string name, string value);
        IRequestUri QueryParam(IDictionary<string,string> map);
    }
}
