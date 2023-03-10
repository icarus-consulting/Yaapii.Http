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

using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Responses
{
    public sealed partial class Response
    {
        /// <summary>
        /// A response from the given parts.
        /// </summary>
        public sealed class Of : MessageEnvelope
        {
            /// <summary>
            /// A response with status 200, reason "OK" and given headers and body.
            /// </summary>
            public Of(IEnumerable<IKvp> headers, string body, params IMessageInput[] extraParts) : this(headers, new TextOf(body), extraParts)
            { }

            /// <summary>
            /// A response with status 200, reason "OK" and given headers and body.
            /// </summary>
            public Of(IEnumerable<IKvp> headers, IText body, params IMessageInput[] extraParts) : this(200,"OK", headers, body, extraParts)
            { }

            /// <summary>
            /// A response with the given status, reason, headers and body.
            /// </summary>
            public Of(int status, string reason, IEnumerable<IKvp> headers, string body, params IMessageInput[] extraParts) : this(status, reason, headers, new TextOf(body), extraParts)
            { }

            /// <summary>
            /// A response with the given status, reason, headers and body.
            /// </summary>
            public Of(int status, string reason, IEnumerable<IKvp> headers, IText body, params IMessageInput[] extraParts) : this(
                new ManyOf<IMessageInput>(
                    new Status(status),
                    new Reason(reason),
                    new Headers(headers),
                    new Body(body),
                    new Parts.Joined(extraParts)
                )
            )
            { }

            /// <summary>
            /// A response with status 200, reason "OK" and given body.
            /// </summary>
            public Of(string body, params IMessageInput[] extraParts) : this(new TextOf(body), extraParts)
            { }

            /// <summary>
            /// A response with status 200, reason "OK" and given body.
            /// </summary>
            public Of(IText body, params IMessageInput[] extraParts) : this(200, "OK", body, extraParts)
            { }

            /// <summary>
            /// A response with the given status, reason and body.
            /// </summary>
            public Of(int status, string reason, string body, params IMessageInput[] extraParts) : this(status, reason, new TextOf(body), extraParts)
            { }

            /// <summary>
            /// A response with the given status, reason and body.
            /// </summary>
            public Of(int status, string reason, IText body, params IMessageInput[] extraParts) : this(
                new ManyOf<IMessageInput>(
                    new Status(status),
                    new Reason(reason),
                    new Body(body),
                    new Parts.Joined(extraParts)
                )
            )
            { }

            /// <summary>
            /// A response with status 200, reason "OK" and given headers.
            /// </summary>
            public Of(IEnumerable<IKvp> headers, params IMessageInput[] extraParts) : this(200, "OK", headers, extraParts)
            { }

            /// <summary>
            /// A response with the given status, reason and headers.
            /// </summary>
            public Of(int status, string reason, IEnumerable<IKvp> headers, params IMessageInput[] extraParts) : this(
                new ManyOf<IMessageInput>(
                    new Status(status),
                    new Reason(reason),
                    new Headers(headers),
                    new Parts.Joined(extraParts)
                )
            )
            { }

            /// <summary>
            /// A response with the given status and reason.
            /// </summary>
            public Of(int status, string reason, params IMessageInput[] extraParts) : this(
                new ManyOf<IMessageInput>(
                    new Status(status),
                    new Reason(reason),
                    new Parts.Joined(extraParts)
                )
            )
            { }

            /// <summary>
            /// A response from the given parts.
            /// Has 200/OK by default, but those can be overwritten.
            /// </summary>
            public Of(params IMessageInput[] responseParts) : this(
                new Yaapii.Atoms.Enumerable.Joined<IMessageInput>(
                    new ManyOf<IMessageInput>(
                        new Status(200),
                        new Reason("OK")
                    ),
                    new ManyOf<IMessageInput>(responseParts)
                )
            )
            { }

            /// <summary>
            /// A response from the given parts.
            /// </summary>
            public Of(IEnumerable<IMessageInput> responseParts) : base(() =>
                new SimpleMessage(responseParts)
            )
            { }
        }
    }
}
