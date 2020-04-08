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

using Newtonsoft.Json.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Parts.Headers;
using Yaapii.JSON;
using Yaapii.Xml;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Adds a body and its content type to a request. 
    /// </summary>
    public sealed partial class Body : MapInput.Envelope
    {
        private const string KEY = "body";

        /// <summary>
        /// Adds a xml body to a request. Sets the Content-Type to 'application/xml'
        /// </summary>
        public Body(IXML xml) : this(
            new TextOf(() => xml.AsNode().ToString()),
            "application/xml"
        )
        { }

        /// <summary>
        /// Adds a json body to a request. Sets the Content-Type to 'application/json'
        /// </summary>
        public Body(JToken json) : this(
            new JSONOf(json)
        )
        { }

        /// <summary>
        /// Adds a json body to a request. Sets the Content-Type to 'application/json'
        /// </summary>
        public Body(IJSON json) : this(
            new TextOf(() => json.Token().ToString()),
            "application/json"
        )
        { }

        /// <summary>
        /// Adds a body to a request
        /// </summary>
        public Body(string text, string contentType) : this(
            new TextOf(text),
            contentType
        )
        { }

        /// <summary>
        /// Adds a body to a request
        /// </summary>
        public Body(string text) : this(new TextOf(text))
        { }

        /// <summary>
        /// Adds a body to a request
        /// </summary>
        public Body(IText text) : this(
            new BytesOf(text)
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        public Body(IText content, string contentType) : this(
            new BytesOf(content),
            contentType
        )
        { }

        /// <summary>
        /// Adds a body body to a request
        /// </summary>
        public Body(IInput content) : this(
            new BytesOf(content)
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        public Body(IInput content, string contentType) : this(
            new BytesOf(content),
            contentType
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        public Body(byte[] content, string contentType) : this(
            new BytesOf(content),
            contentType
        )
        { }

        /// <summary>
        /// Adds a body to a request. 
        /// </summary>
        public Body(byte[] content) : this(
            new BytesOf(content)
        )
        { }

        /// <summary>
        /// Adds a body to a request. 
        /// </summary>
        public Body(IBytes content) : this(
            new MapInput.Of(
                new Kvp.Of(KEY, () =>
                    new TextOf(
                        new BytesBase64(content)
                    ).AsString()
                )
            )
        )
        { }

        /// <summary>
        /// Adds a body and its content type to a request. 
        /// </summary>
        public Body(IBytes content, string contentType) : this(
            new MapInput.Of(
                new Kvp.Of(KEY, () =>
                    new TextOf(
                        new BytesBase64(content)
                    ).AsString()
                )
            ),
            new ContentType(contentType)
        )
        { }
        
        private Body(params IMapInput[] inputs) : base(()=>
            new Parts.Joined(inputs)
        )
        { }


    }
}
