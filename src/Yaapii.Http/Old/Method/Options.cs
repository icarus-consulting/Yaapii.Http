using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Http.MethodX
{
    public sealed class Options : IMethod
    {
        private readonly string _name;

        public Options()
        {
            _name = "OPTION";
        }

        public string Name()
        {
            return _name;
        }

        public override bool Equals(object obj)
        {
            return obj is IMethod && (obj as IMethod).Name() == _name;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }
}
