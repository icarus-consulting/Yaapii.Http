using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;

namespace Yaapii.Http
{
    public sealed class EmptyResponse : IResponse
    {
        public IRequest Back()
        {
            throw new UnsupportedException("Back() is unsupported - its an empty request");
        }

        public IInput Body()
        {
            throw new UnsupportedException("Body() is unsupported - its an empty request");
        }

        public IDictionary<string, IList<string>> Headers()
        {
            throw new UnsupportedException("Headers() is unsupported - its an empty request");
        }

        public IText Reason()
        {
            throw new UnsupportedException("Reason() is unsupported - its an empty request");
        }

        public int Status()
        {
            throw new UnsupportedException("Status() is unsupported - its an empty request");
        }
    }
}
