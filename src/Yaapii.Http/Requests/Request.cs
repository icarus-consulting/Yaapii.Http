﻿//MIT License

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
using Yaapii.Atoms.Enumerable;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Requests
{
    /// <summary>
    /// A request from the given parts.
    /// </summary>
    public sealed class Request : MessageEnvelope
    {
        /// <summary>
        /// A request from the given parts.
        /// </summary>
        public Request(params IMessageInput[] parts) : this(
            new ManyOf<IMessageInput>(parts)
        )
        { }

        /// <summary>
        /// A request with the specified method and the given parts.
        /// </summary>
        public Request(string method, params IMessageInput[] parts) : this(
            new Joined<IMessageInput>(
                new ManyOf<IMessageInput>(parts),
                new Method(method)
            )
        )
        { }

        /// <summary>
        /// A request with the specified <see cref="System.Uri"/> and the given parts.
        /// </summary>
        public Request(Uri uri, params IMessageInput[] parts) : this(
            new Joined<IMessageInput>(
                new ManyOf<IMessageInput>(parts),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with the specified method, <see cref="System.Uri"/> and the given parts.
        /// </summary>
        public Request(string method, Uri uri, params IMessageInput[] parts) : this(
            new Joined<IMessageInput>(
                new ManyOf<IMessageInput>(parts),
                new Method(method),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request with the specified method, <see cref="System.Uri"/> and the given parts.
        /// </summary>
        public Request(string method, string uri, params IMessageInput[] parts) : this(
            new Joined<IMessageInput>(
                new ManyOf<IMessageInput>(parts),
                new Method(method),
                new Address(uri)
            )
        )
        { }

        /// <summary>
        /// A request from the given parts.
        /// </summary>
        public Request(IEnumerable<IMessageInput> parts) : base(
            new SimpleMessage(parts)
        )
        { }
    }
}
