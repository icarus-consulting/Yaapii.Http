using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Dict;

namespace Yaapii.Http.Wire.AspNet
{
    public sealed class AspWire : IWire2
    {
        public AspWire(IAspClient client)
        {
            throw new NotImplementedException();
        }

        public IDict Response(IDict request)
        {
            var response = new HashDict();

            //use DictOf + DictInput objects to build response
            throw new NotImplementedException();
        }
    }
}
