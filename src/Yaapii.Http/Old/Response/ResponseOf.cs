using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Response
{
    public sealed class ResponseOf : IResponse
    {
        private readonly IRequest _req;
        private readonly int _code;
        private readonly IText _phrase;
        private readonly IEnumerable<KeyValuePair<string, string>> _hdrs;
        private readonly IBytes _content;

        public ResponseOf(IRequest req, int status, IText reason, IEnumerable<KeyValuePair<string,string>> headers, IBytes body)
        {
            _req = req;
            _code = status;
            _phrase = reason;
            _hdrs = headers;
            _content = body;
        }

        public IRequest Back()
        {
            return this._req;
        }

        public IInput Body()
        {
            return new InputOf(this._content); //@TODO: Maybe check for UTF-8 error marker (\uFFFD).
        }

        public IDictionary<string, IList<string>> Headers()
        {
            var map = new ConcurrentDictionary<string, IList<string>>();
            foreach (var kvp in this._hdrs)
            {
                map.AddOrUpdate(
                    kvp.Key,
                    new List<string>(),
                    (k, v) => 
                    {
                        v.Add(kvp.Value);
                        return v;
                    }
                );
            }

            return map;
        }

        public IText Reason()
        {
            return this._phrase;
        }

        public int Status()
        {
            return this._code;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this._code)
                .Append(' ')
                .Append(this._phrase)
                .Append($" [{this.Back().Uri().Value().AbsoluteUri}]\n");

            foreach (var header in this._hdrs)
            {
                sb.Append($"{header.Key}: {header.Value}");
            }

            sb.Append("\n")
                .Append(new TextOf(this._content).AsString());

            return sb.ToString();
        }
    }
}
