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

using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds a query parameter to a request.
    /// </summary>
    public sealed partial class QueryParam : MapInput.Envelope
    {
        private const string KEY_PREFIX = "query:";

        /// <summary>
        /// Adds a query parameter to a request.
        /// </summary>
        public QueryParam(string key, string value) : this(
            () => key,
            () => value
        )
        { }

        /// <summary>
        /// Adds a query parameter to a request.
        /// </summary>
        public QueryParam(IText key, IText value) : this(
            () => key.AsString(),
            () => value.AsString()
        )
        { }

        /// <summary>
        /// Adds a query parameter to a request.
        /// </summary>
        public QueryParam(IScalar<string> key, IScalar<string> value) : this(
            () => key.Value(),
            () => value.Value()
        )
        { }

        /// <summary>
        /// Adds a query parameter to a request.
        /// </summary>
        public QueryParam(Func<string> key, Func<string> value) : base(
            new Kvp.Of(
                new TextOf(() => $"{KEY_PREFIX}{key()}"),
                value
            )
        )
        { }
    }
}
