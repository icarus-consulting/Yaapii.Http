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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;
using Yaapii.Http.Parts.Headers;
using Yaapii.JSON;
using Yaapii.Xml;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a body and its content type to a request. 
    /// </summary>
    public sealed partial class Body : MessageInputEnvelope
    {
        private const string KEY = "body";

        /// <summary>
        /// Adds a xml body to a request. Sets the Content-Type to 'application/xml'
        /// </summary>
        public Body(IXML xml, bool overwriteExisting = false) : this(
            new TextOf(() => xml.AsNode().ToString()),
            "application/xml",
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a json body to a request. Sets the Content-Type to 'application/json'
        /// </summary>
        public Body(JToken json, bool overwriteExisting = false) : this(
            new JSONOf(json),
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a json body to a request. Sets the Content-Type to 'application/json'
        /// </summary>
        public Body(IJSON json, bool overwriteExisting = false) : this(
            new TextOf(() => json.Token().ToString()),
            "application/json",
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body to a request
        /// </summary>
        public Body(string text, string contentType, bool overwriteExisting = false) : this(
            new InputOf(text),
            contentType,
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body to a request
        /// </summary>
        public Body(string text, bool overwriteExisting = false) : this(
            new InputOf(text),
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body to a request
        /// </summary>
        public Body(IText text, bool overwriteExisting = false) : this(
            new InputOf(text),
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        public Body(IText content, string contentType, bool overwriteExisting = false) : this(
            new InputOf(content),
            contentType,
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body body to a request
        /// </summary>
        public Body(IBytes content, bool overwriteExisting = false) : this(
            new InputOf(content),
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        public Body(IBytes content, string contentType, bool overwriteExisting = false) : this(
            new InputOf(content),
            contentType,
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        public Body(byte[] content, string contentType, bool overwriteExisting = false) : this(
            new InputOf(content),
            contentType,
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body to a request. 
        /// </summary>
        public Body(byte[] content, bool overwriteExisting = false) : this(
            new InputOf(content),
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body to a request. 
        /// </summary>
        public Body(IInput content, bool overwriteExisting = false) : this(
            content,
            new ManyOf<IMessageInput>(),
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        public Body(IInput content, string contentType, bool overwriteExisting = false) : this(
            content,
            new ManyOf<IMessageInput>(
                new ContentType(contentType)
            ),
            overwriteExisting
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        private Body(IInput content, IEnumerable<IMessageInput> additional, bool overwriteExisting) : base(
            new Joined(
                new SimpleMessageInput((message) =>
                {
                    if(message.HasBody() && !overwriteExisting)
                    {
                        throw new InvalidOperationException("Can not add body to message. It already has a body.");
                    }
                    return
                        new SimpleMessage(
                            message.Head(),
                            content
                        );
                }),
                new Joined(additional)
            )
        )
        { }
    }
}
