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
using Yaapii.Atoms;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Fake
{
    /// <summary>
    /// A fake <see cref="IMessage"/>.
    /// </summary>
    public sealed class FkMessage : IMessage
    {
        private readonly Func<IDictionary<string, string>> head;
        private readonly Func<bool> hasBody;
        private readonly Func<IInput> body;

        /// <summary>
        /// A fake <see cref="IMessage"/>.
        /// </summary>
        public FkMessage() : this(
            () => new MapOf<string, string>(),
            () => false,
            () => new DeadInput()
        )
        { }

        /// <summary>
        /// A fake <see cref="IMessage"/>.
        /// </summary>
        public FkMessage(Func<IDictionary<string, string>> head, Func<bool> hasBody, Func<IInput> body)
        {
            this.head = head;
            this.hasBody = hasBody;
            this.body = body;
        }

        public IDictionary<string, string> Head()
        {
            return this.head.Invoke();
        }

        public bool HasBody()
        {
            return this.hasBody.Invoke();
        }

        public IInput Body()
        {
            return this.body.Invoke();
        }
    }
}