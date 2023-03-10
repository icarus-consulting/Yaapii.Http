//MIT License

//Copyright(c) 2023 ICARUS Consulting GmbH

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
using System.Threading.Tasks;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Http.Facets;
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
        private readonly IMessage template;
        private readonly IWire wire;

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path.
        /// </summary>
        public Match(string path, Action<IMessage> requestAction) : this(
            path,
            new FkWire(requestAction)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path.
        /// </summary>
        public Match(string path, Func<IMessage, string> responseBody) : this(
            path,
            new FkWire(responseBody)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path.
        /// </summary>
        public Match(string path, Func<IMessage, IMessage> response) : this(
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
        public Match(string path, IMessageInput template, Action<IMessage> requestAction) : this(
            path,
            template,
            new FkWire(requestAction)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path and additional template parts.
        /// </summary>
        public Match(string path, IMessageInput template, Func<IMessage, string> responseBody) : this(
            path,
            template,
            new FkWire(responseBody)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path and additional template parts.
        /// </summary>
        public Match(string path, IMessageInput template, Func<IMessage, IMessage> response) : this(
            path,
            template,
            new FkWire(response)
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified path and additional template parts.
        /// </summary>
        public Match(string path, IMessageInput template, IWire wire) : this(
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
        public Match(IMessageInput template, IWire wire) : this(
            new SimpleMessage(template),
            wire
        )
        { }

        /// <summary>
        /// A wire template that applies to a request, if the request has the specified parts.
        /// </summary>
        public Match(IMessage template, IWire wire)
        {
            this.template = template;
            this.wire = wire;
        }

        public bool Applies(IMessage request)
        {
            return
                HasTemplateHeaders(request)
                && HasNonHeaderParts(request)
                && HasBody(request);
        }

        public Task<IMessage> Response(IMessage request)
        {
            return this.wire.Response(request);
        }

        /// <summary>
        /// Headers are numbered to allow multiple values for the same key.
        /// Those numbers may differ between template and request,
        /// so headers can not be compared as raw request parts.
        /// </summary>
        private bool HasTemplateHeaders(IMessage request)
        {
            var templateHeaders = new Headers.Of(this.template);
            var requestHeaders = new Headers.Of(request);
            var applies = true;
            foreach (var templateHeader in templateHeaders)
            {
                var matches =
                    new Yaapii.Atoms.Enumerable.Filtered<IKvp>(kvp =>
                        kvp.Key() == templateHeader.Key() && kvp.Value() == templateHeader.Value(),
                        requestHeaders
                    );
                if (new Yaapii.Atoms.Enumerable.LengthOf(matches).Value() == 0)
                {
                    applies = false;
                    break;
                }
            }
            return applies;
        }

        private bool HasNonHeaderParts(IMessage request)
        {
            var templateParts =
                new MapOf(
                    new FilteredDictionary((key, value) =>
                        !key.StartsWith(HEADER_KEY_PREFIX),
                        this.template.Head()
                    )
                );
            var requestParts =
                new FilteredDictionary((key, value) =>
                    !key.StartsWith(HEADER_KEY_PREFIX),
                    request.Head()
                );
            var applies = true;
            foreach (var templatePart in templateParts.Keys)
            {
                var matches =
                    new FilteredDictionary((key, value) =>
                        key == templatePart && value() == templateParts[templatePart],
                        requestParts
                    );
                if (new LengthOf(matches.Keys).Value() == 0)
                {
                    applies = false;
                    break;
                }
            }
            return applies;
        }

        private bool HasBody(IMessage request)
        {
            var result = true;
            if (this.template.HasBody() && request.HasBody())
            {
                var templateBytes = new BytesOf(this.template.Body()).AsBytes();
                var requestBytes = new BytesOf(request.Body()).AsBytes();
                if(templateBytes.Length != requestBytes.Length)
                {
                    result = false;
                }
                else
                {
                    for(int i = 0; i < templateBytes.Length; i++)
                    {
                        if (templateBytes[i] != requestBytes[i])
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
