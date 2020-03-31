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

using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Xml;

namespace Yaapii.Http.Responses
{
    public sealed partial class XmlResponse
    {
        /// <summary>
        /// A Response containing the given status, reason and a body from the given xml.
        /// </summary>
        public sealed class Of : Map.Envelope
        {
            /// <summary>
            /// A 200/OK Response containing the given xml.
            /// </summary>
            public Of(IXML body, params IMapInput[] extraParts) : this(200, "OK", body, extraParts)
            { }

            /// <summary>
            /// A Response containing the given status, reason and a body from the given xml.
            /// </summary>
            public Of(int status, string reason, IXML body, params IMapInput[] extraParts) : base(() =>
                new Response.Of(
                    new Status(status),
                    new Reason(reason),
                    new XmlBody(body),
                    new Parts.Joined(extraParts)
                )
            )
            { }
        }
    }
}
