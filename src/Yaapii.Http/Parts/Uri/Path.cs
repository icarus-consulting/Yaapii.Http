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
using Yaapii.Atoms;
using Yaapii.Atoms.Text;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds the path of a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class Path : MessageInputEnvelope
    {
        private const string KEY = "path";

        /// <summary>
        /// Adds the path of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Path(string path) : this(
            new TextOf(path)
        )
        { }

        /// <summary>
        /// Adds the path of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Path(IScalar<string> path) : this(
            new TextOf(path)
        )
        { }

        /// <summary>
        /// Adds the path of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Path(Func<string> path) : this(
            new TextOf(path)
        )
        { }

        /// <summary>
        /// Adds the path of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Path(IText path) : base(
            new KvpOf(KEY, () =>
            {
                var result = path.AsString();
                if (!result.StartsWith("/"))
                {
                    result = $"/{result}";
                }
                return result;
            })
        )
        { }
    }
}
