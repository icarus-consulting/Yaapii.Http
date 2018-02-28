using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Yaapii.Http.RequestUri
{
    public sealed class BaseUri : IRequestUri
    {
        private readonly IRequest _req;
        private readonly Uri _url;

        public BaseUri(IRequest req, Uri url)
        {
            _req = req;
            _url = url;
        }
        public IRequest Back()
        {
            return this._req.Uri(this._url);
        }

        public IRequestUri Path(string segment)
        {
            return 
                new BaseUri(
                    this._req,
                    new System.Uri(
                        this._url,
                        segment
                    )
                );
        }

        public IRequestUri Port(int num)
        {
           var uri = new System.UriBuilder(this._url);
           uri.Port = num;
           return new BaseUri(this._req,uri.Uri);
        }

        public IRequestUri QueryParam(string name, string value)
        {
            var uri = new UriBuilder(this._url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            query[name] = value;
            uri.Query = query.ToString();
            return new BaseUri(this._req, uri.Uri);
        }

        public IRequestUri QueryParam(IDictionary<string, string> map)
        {
            IRequestUri uri = this;
            foreach (var kvp in map)
            {
                uri = uri.QueryParam(kvp.Key, kvp.Value);
            }

            return uri;
        }

        public IRequestUri Uri(Uri url)
        {
            return new BaseUri(this._req, url);
        }

        public Uri Value()
        {
            return this._url;
        }
    }
}
