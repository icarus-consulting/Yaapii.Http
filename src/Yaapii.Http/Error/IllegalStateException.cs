using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Error
{
    public sealed class IllegalStateException : Exception
    {
        public IllegalStateException(IText unformatted, params string[] parts) : base(new FormattedText(unformatted, parts).AsString())
        { }

        public IllegalStateException(Exception inner, IText unformatted, params string[] parts) : base(new FormattedText(unformatted, parts).AsString(), inner)
        { }

        public IllegalStateException(string msg) : base(msg)
        { }
        public IllegalStateException(string msg, Exception ex) : base(msg, ex)
        { }
    }
}
