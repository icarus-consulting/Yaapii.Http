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
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http
{
    /// <summary>
    /// A simple <see cref="IMessage"/>.
    /// </summary>
    public sealed class SimpleMessage : IMessage
    {
        private readonly IScalar<IDictionary<string, string>> head;
        private readonly IScalar<bool> hasBody;
        private readonly IScalar<IInput> body;

        /// <summary>
        /// A simple <see cref="IMessage"/>.
        /// </summary>
        public SimpleMessage() : this(
            new Live<IDictionary<string, string>>(new MapOf<string, string>()),
            new Live<bool>(false),
            new Live<IInput>(new DeadInput())
        )
        { }

        /// <summary>
        /// A simple <see cref="IMessage"/>.
        /// </summary>
        public SimpleMessage(IDictionary<string, string> head) : this(
            new Live<IDictionary<string, string>>(head),
            new Live<bool>(false),
            new Live<IInput>(new DeadInput())
        )
        { }

        /// <summary>
        /// A simple <see cref="IMessage"/>.
        /// </summary>
        public SimpleMessage(IInput body) : this(
            new Live<IDictionary<string, string>>(new MapOf<string, string>()),
            new Live<bool>(true),
            new Live<IInput>(body)
        )
        { }

        /// <summary>
        /// A simple <see cref="IMessage"/>.
        /// </summary>
        public SimpleMessage(IDictionary<string, string> head, IInput body) : this(
            new Live<IDictionary<string, string>>(head),
            new Live<bool>(true),
            new Live<IInput>(body)
        )
        { }

        /// <summary>
        /// A simple <see cref="IMessage"/>.
        /// </summary>
        public SimpleMessage(params IMessageInput[] inputs) : this(
            new ManyOf<IMessageInput>(inputs)
        )
        { }

        /// <summary>
        /// A simple <see cref="IMessage"/>.
        /// </summary>
        public SimpleMessage(IEnumerable<IMessageInput> inputs) : this(
            new ScalarOf<IMessage>(() =>
            {
                IMessage message = new SimpleMessage();
                foreach(var input in inputs)
                {
                    message = input.Apply(message);
                }
                return message;
            })
        )
        { }

        /// <summary>
        /// A simple <see cref="IMessage"/>.
        /// </summary>
        private SimpleMessage(IScalar<IMessage> message) : this(
            new ScalarOf<IDictionary<string, string>>(() => message.Value().Head()),
            new ScalarOf<bool>(() => message.Value().HasBody()),
            new ScalarOf<IInput>(() => message.Value().Body())
        )
        { }

        /// <summary>
        /// A simple <see cref="IMessage"/>.
        /// </summary>
        private SimpleMessage(IScalar<IDictionary<string, string>> head, IScalar<bool> hasBody, IScalar<IInput> body)
        {
            this.head = head;
            this.hasBody = hasBody;
            this.body = body;
        }

        public IDictionary<string, string> Head()
        {
            return this.head.Value();
        }

        public bool HasBody()
        {
            return this.hasBody.Value();
        }

        public IInput Body()
        {
            return this.body.Value();
        }
    }
}
