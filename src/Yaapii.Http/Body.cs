using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;
using Yaapii.Http.Error;

namespace Yaapii.Http
{
    public sealed class RequestBodyOf : IRequestBody
    {
        private readonly IRequest request;
        private readonly IBytes content;
        private readonly string prepend;
        private readonly Encoding encoding = Encoding.UTF8;

        public RequestBodyOf(IRequest request, IBytes content, string prepend = "")
        {
            this.request = request;
            this.content = content;
            this.prepend = prepend;
        }

        public IRequest Back()
        {
            return request;
        }

        public IText Content()
        {
            return new TextOf(this.content, encoding);
        }

        public IRequestBody With(IText body)
        {
            return new RequestBodyOf(request, new BytesOf(body));
        }

        public IRequestBody FormParam(string name, string value)
        {
            try
            {
                return
                    new RequestBodyOf(
                        this.request,
                        new BytesOf(
                            new StringBuilder(
                                this.Content().AsString()
                            )
                            .Append(this.prepend)
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
                        new Formatted("cannot url encode '{0}': {1}", value, ex.Message).AsString(),
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
