//MIT License

//Copyright(c) 2020 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Mock.Templates
{
    /// <summary>
    /// A wire template that applies to a request, if the request has the specified parts.
    /// </summary>
    public sealed class Match : ITemplate
    {
        private const string HEADER_KEY_PREFIX = "header:";
        private readonly IDictionary<string, string> template;
        private readonly IWire wire;

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path.
        /// </summary>
        public Match(string path, Action<IDictionary<string, string>> requestAction) : this(
            path,
            new FkWire(requestAction)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path.
        /// </summary>
        public Match(string path, Func<IDictionary<string, string>, string> responseBody) : this(
            path,
            new FkWire(responseBody)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path.
        /// </summary>
        public Match(string path, Func<IDictionary<string, string>, IDictionary<string, string>> response) : this(
            path,
            new FkWire(response)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path.
        /// </summary>
        public Match(string path, IWire wire) : this(
            new Parts.Uri.Path(path),
            wire
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path and additional template parts.
        /// </summary>
        public Match(string path, IMapInput template, Action<IDictionary<string, string>> requestAction) : this(
            path,
            template,
            new FkWire(requestAction)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path and additional template parts.
        /// </summary>
        public Match(string path, IMapInput template, Func<IDictionary<string, string>, string> responseBody) : this(
            path,
            template,
            new FkWire(responseBody)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path and additional template parts.
        /// </summary>
        public Match(string path, IMapInput template, Func<IDictionary<string, string>, IDictionary<string, string>> response) : this(
            path,
            template,
            new FkWire(response)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path and additional template parts.
        /// </summary>
        public Match(string path, IMapInput template, IWire wire) : this(
            new Parts.Joined(
                new Parts.Uri.Path(path),
                template
            ),
            wire
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified parts.
        /// </summary>
        public Match(IMapInput template, IWire wire) : this(new Map.Of(template), wire)
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified parts.
        /// </summary>
        public Match(IDictionary<string, string> template, IWire wire)
        {
            this.template = template;
            this.wire = wire;
        }

        public bool Applies(IDictionary<string, string> request)
        {
            return HasTemplateHeaders(request) && HasNonHeaderParts(request);
        }

        public IDictionary<string, string> Response(IDictionary<string, string> request)
        {
            return this.wire.Response(request);
        }

        /// <summary>
        /// Headers are numbered to allow multiple values for the same key.
        /// Those numbers may differ between template and request,
        /// so headers can not be compared as raw request parts.
        /// </summary>
        private bool HasTemplateHeaders(IDictionary<string, string> request)
        {
            var templateHeaders = new Headers.Of(this.template);
            var requestHeaders = new Headers.Of(request);
            var applies = true;
            foreach(var templateHeader in templateHeaders)
            {
                var matches =
                    new Yaapii.Http.AtomsTemp.Enumerable.Filtered<IKvp>(kvp =>
                        kvp.Key() == templateHeader.Key() && kvp.Value() == templateHeader.Value(),
                        requestHeaders
                    );
                if(new Yaapii.Http.AtomsTemp.Enumerable.LengthOf(matches).Value() == 0)
                {
                    applies = false;
                    break;
                }
            }
            return applies;
        }

        private bool HasNonHeaderParts(IDictionary<string, string> request)
        {
            var templateParts =
                new AtomsTemp.Enumerable.Filtered<KeyValuePair<string, string>>(kvp =>
                    !kvp.Key.StartsWith(HEADER_KEY_PREFIX),
                    this.template
                );
            var requestParts =
                new AtomsTemp.Enumerable.Filtered<KeyValuePair<string, string>>(kvp =>
                    !kvp.Key.StartsWith(HEADER_KEY_PREFIX),
                    request
                );
            var applies = true;
            foreach(var templatePart in templateParts)
            {
                var matches =
                    new Yaapii.Http.AtomsTemp.Enumerable.Filtered<KeyValuePair<string, string>>(kvp =>
                        kvp.Key == templatePart.Key && kvp.Value == templatePart.Value,
                        requestParts
                    );
                if (new Yaapii.Http.AtomsTemp.Enumerable.LengthOf(matches).Value() == 0)
                {
                    applies = false;
                    break;
                }
            }
            return applies;
        }
    }
}
