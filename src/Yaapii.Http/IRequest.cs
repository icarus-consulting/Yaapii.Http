using System;
using Yaapii.Atoms;

namespace Yaapii.Http
{
    public interface IRequest
    {
        IRequestUri Uri();
        IRequest Uri(Uri url);
        IRequestBody Body();
        IRequest Method(IMethod verb);
        IRequest Header(string name, string value);
        //maybe we can use two timeouts, for connection and for reading data(?)
        IRequest Timeout(TimeSpan timeout);
        IResponse Response();
        IResponse Response(IInput input);
        ///**
        // * Send it through a decorating {@link Wire}.
        // */
        //IRequest Through<T>(Type type, params Object[] args);
    }
}
