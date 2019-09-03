using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Http.MethodX
{
    public sealed class Head : IMethod
    {
        private readonly string _name;

        public Head()
        {
            _name = "HEAD";
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
