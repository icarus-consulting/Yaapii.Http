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
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Responses;

namespace Yaapii.Http.Fake
{
    /// <summary>
    /// A fake wire to convert http requests into responses.
    /// </summary>
    public sealed class FkWire : Wires.WireEnvelope
    {
        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Creates a response from the given parts.
        /// Returns 200/OK by default, but those can be overwritten.
        /// </summary>
        public FkWire(params IMessageInput[] responseParts) : this(
            200, 
            "OK", 
            responseParts
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code and reason.
        /// </summary>
        public FkWire(int status, string reason, params IMessageInput[] extraParts) : this(
            new ManyOf<IMessageInput>(
                new Status(status),
                new Reason(reason),
                new Parts.Joined(extraParts)
            )
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns 200/OK and the specified body.
        /// </summary>
        public FkWire(string body, params IMessageInput[] extraParts) : this(
            new TextOf(body),
            extraParts
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns 200/OK and the specified body.
        /// </summary>
        public FkWire(IText body, params IMessageInput[] extraParts) : this(
            200,
            "OK",
            body,
            extraParts
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code, reason and body.
        /// </summary>
        public FkWire(int status, string reason, string body, params IMessageInput[] extraParts) : this(
            status,
            reason,
            new TextOf(body),
            extraParts
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code, reason and body.
        /// </summary>
        public FkWire(int status, string reason, IText body, params IMessageInput[] extraParts) : this(
            new ManyOf<IMessageInput>(
                new Status(status),
                new Reason(reason),
                new Body(body),
                new Parts.Joined(extraParts)
            )
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns 200/OK and the specified headers.
        /// </summary>
        public FkWire(IDictionary<string, string> headers, params IMessageInput[] extraParts) : this(
            200,
            "OK",
            headers,
            extraParts
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code, reason and headers.
        /// </summary>
        public FkWire(int status, string reason, IDictionary<string, string> headers, params IMessageInput[] extraParts) : this(
            new ManyOf<IMessageInput>(
                new Status(status),
                new Reason(reason),
                new Headers(headers),
                new Parts.Joined(extraParts)
            )
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns 200/OK and the specified headers and body.
        /// </summary>
        public FkWire(IDictionary<string, string> headers, string body, params IMessageInput[] extraParts) : this(
            headers,
            new TextOf(body),
            extraParts
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns 200/OK and the specified headers and body.
        /// </summary>
        public FkWire(IDictionary<string, string> headers, IText body, params IMessageInput[] extraParts) : this(
            200,
            "OK",
            headers,
            body,
            extraParts
        )
        { }
        
        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code, reason, headers and body.
        /// </summary>
        public FkWire(int status, string reason, IDictionary<string, string> headers, string body, params IMessageInput[] extraParts) : this(
            status,
            reason,
            headers,
            new TextOf(body),
            extraParts
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Returns the specified status code, reason, headers and body.
        /// </summary>
        public FkWire(int status, string reason, IDictionary<string, string> headers, IText body, params IMessageInput[] extraParts) : this(
            new ManyOf<IMessageInput>(
                new Status(status),
                new Reason(reason),
                new Headers(headers),
                new Parts.Joined(extraParts)
            )
        )
        { }
        
        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Creates a response from the given parts.
        /// </summary>
        public FkWire(IEnumerable<IMessageInput> responseParts) : this(req => 
            new SimpleMessage(responseParts)
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Executes the given action for each request and returns 200/OK.
        /// </summary>
        public FkWire(Action<IMessage> requestAction) : this(req =>
        {
            requestAction(req);
            return new Task<IMessage>(() => new Response.Of(200, "OK"));
        })
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Creates a response using the given function.
        /// </summary>
        public FkWire(Func<IMessage, string> responseBody) : this(req => 
            new Response.Of(200, "OK", responseBody(req))
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Creates a response using the given function.
        /// </summary>
        public FkWire(Func<IMessage, IMessage> syncResponse) : this(req =>
            new Task<IMessage>(() => syncResponse(req))
        )
        { }

        /// <summary>
        /// A fake wire to convert http requests into responses.
        /// Creates a response using the given function.
        /// </summary>
        public FkWire(Func<IMessage, Task<IMessage>> response) : base((req) => {
            var task = response(req);
            task.Start();
            return task;
        })
        { }
    }
}
