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
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http
{
    /// <summary>
    /// Envelope for an <see cref="IMessageInput"/>.
    /// </summary>
    public abstract class MessageInputEnvelope : IMessageInput
    {
        private readonly IScalar<IMessageInput> messageInput;

        /// <summary>
        /// Envelope for an <see cref="IMessageInput"/>.
        /// </summary>
        public MessageInputEnvelope(params IKvp[] headInputs) : this(
            new SimpleMessageInput(headInputs)
        )
        { }

        /// <summary>
        /// Envelope for an <see cref="IMessageInput"/>.
        /// </summary>
        public MessageInputEnvelope(IEnumerable<IKvp> headInputs) : this(
            new SimpleMessageInput(headInputs)
        )
        { }

        /// <summary>
        /// Envelope for an <see cref="IMessageInput"/>.
        /// </summary>
        public MessageInputEnvelope(params IMapInput[] headInputs) : this(
            new SimpleMessageInput(headInputs)
        )
        { }

        /// <summary>
        /// Envelope for an <see cref="IMessageInput"/>.
        /// </summary>
        public MessageInputEnvelope(IEnumerable<IMapInput> headInputs) : this(
            new SimpleMessageInput(headInputs)
        )
        { }

        /// <summary>
        /// Envelope for an <see cref="IMessageInput"/>.
        /// </summary>
        public MessageInputEnvelope(Func<IMessage, IMessage> apply) : this(
            new SimpleMessageInput(apply)
        )
        { }

        /// <summary>
        /// Envelope for an <see cref="IMessageInput"/>.
        /// </summary>
        public MessageInputEnvelope(IMessageInput messageInput) : this(
            new Live<IMessageInput>(messageInput)
        )
        { }

        /// <summary>
        /// Envelope for an <see cref="IMessageInput"/>.
        /// </summary>
        public MessageInputEnvelope(Func<IMessageInput> messageInput) : this(
            new ScalarOf<IMessageInput>(messageInput)
        )
        { }

        /// <summary>
        /// Envelope for an <see cref="IMessageInput"/>.
        /// </summary>
        public MessageInputEnvelope(IScalar<IMessageInput> messageInput)
        {
            this.messageInput = messageInput;
        }

        public IMessage Apply(IMessage message)
        {
            return this.messageInput.Value().Apply(message);
        }
    }
}
