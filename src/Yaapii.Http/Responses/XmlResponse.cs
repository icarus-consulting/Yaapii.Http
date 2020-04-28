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

using System.Collections.Generic;
using System.Xml.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Map;
using Yaapii.Http.Parts.Bodies;
using Yaapii.Http.Wires;
using Yaapii.Xml;

namespace Yaapii.Http.Responses
{
    /// <summary>
    /// XML data received as a response from the given wire.
    /// </summary>
    public sealed partial class XmlResponse : IXML
    {
        private readonly IScalar<IXML> xml;

        /// <summary>
        /// XML data received as a response from the given wire.
        /// </summary>
        public XmlResponse(IWire wire, IVerification verification) : this(new Verified(wire, verification))
        { }

        /// <summary>
        /// XML data received as a response from the given wire.
        /// </summary>
        public XmlResponse(IWire wire) : this(wire, new MapOf(new MapInputOf()))
        { }

        /// <summary>
        /// XML data received as a response from the given wire.
        /// </summary>
        public XmlResponse(IWire wire, IVerification verification, IDictionary<string, string> request) : this(new Verified(wire, verification), request)
        { }

        /// <summary>
        /// XML data received as a response from the given wire.
        /// </summary>
        public XmlResponse(IWire wire, IDictionary<string, string> request)
        {
            this.xml =
                new ScalarOf<IXML>(() =>
                    new XMLCursor(
                        new Body.Of(
                            wire.Response(request)
                        )
                    )
                );
        }

        public XNode AsNode()
        {
            return this.xml.Value().AsNode();
        }

        public IList<IXML> Nodes(string query)
        {
            return this.xml.Value().Nodes(query);
        }

        public IList<string> Values(string query)
        {
            return this.xml.Value().Values(query);
        }

        public IXML WithNamespace(string prefix, object uri)
        {
            return this.xml.Value().WithNamespace(prefix, uri);
        }
    }
}