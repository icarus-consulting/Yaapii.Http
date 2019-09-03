using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Response
{
    public sealed class ResponseEnvelope : IResponse2
    {
        private readonly IScalar<IDict> response;

        public ResponseEnvelope(Func<IDict> response): this(
            new ScalarOf<IDict>(response)
        )
        { }

        public ResponseEnvelope(IScalar<IDict> response)
        {
            this.response = response;
        }

        public IDict Dict()
        {
            return this.response.Value();
        }

        public void Touch()
        {
            this.Dict().Entries();
        }
    }
}
