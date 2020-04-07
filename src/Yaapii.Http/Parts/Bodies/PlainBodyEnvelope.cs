﻿//MIT License

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

using Yaapii.Atoms;
using Yaapii.Atoms.Text;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies
{
    /// <summary>
    /// Envelope for adding a plain text body to a request.
    /// </summary>
    public abstract class PlainBodyEnvelope : MapInput.Envelope
    {
        private const string KEY = "body";

        /// <summary>
        /// Envelope for adding a plain text body to a request.
        /// </summary>
        public PlainBodyEnvelope(string content, params IMapInput[] extraParts) : this(new TextOf(content), extraParts)
        { }

        /// <summary>
        /// Envelope for adding a plain text body to a request.
        /// </summary>
        protected PlainBodyEnvelope(IText content, params IMapInput[] extraParts) : base(() =>
            new Parts.Joined(
                new MapInput.Of(
                    new Kvp.Of(KEY, () => content.AsString())
                ),
                new Parts.Joined(extraParts)
            )
        )
        { }
    }
}
