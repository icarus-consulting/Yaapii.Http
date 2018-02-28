using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using Yaapii.Atoms;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;
using Yaapii.Http.Error;

namespace Yaapii.Http
{
    public sealed class RequestBodyOf : IRequestBody
    {
        private readonly IRequest _request;
        private readonly IBytes _content;
        private readonly string _prepend;
        private readonly Encoding _encoding = Encoding.UTF8;

        public RequestBodyOf(IRequest request, IBytes content, string prepend = "")
        {
            _request = request;
            _content = content;
            _prepend = prepend;
        }

        public IRequest Back()
        {
            return _request;
        }

        public IText Content()
        {
            return new TextOf(this._content, _encoding);
        }

        public IRequestBody With(IText body)
        {
            return new RequestBodyOf(_request, new BytesOf(body));
        }

        public IRequestBody FormParam(string name, string value)
        {
            try
            {
                return
                    new RequestBodyOf(
                        this._request,
                        new BytesOf(
                            new StringBuilder(
                                this.Content().AsString()
                            )
                            .Append(this._prepend)
                            .Append(name)
                            .Append('=')
                            .Append(UrlEncoder.Default.Encode(value)).ToString()
                        ),
                        "&"
                    );

            }
            catch (ArgumentException ex)
            {
                throw
                    new IllegalStateException(
                        new FormattedText("cannot url encode '{0}': {1}", value, ex.Message).AsString(),
                        ex
                    );
            }
        }

        public IRequestBody FormParam(IDictionary<string, string> param)
        {
            IRequestBody body = this;
            foreach(var entry in param)
            {
                body = body.FormParam(entry.Key, entry.Value);
            }
            return body;
        }

        public override string ToString()
        {
            return Content().AsString();
        }


    }
}
