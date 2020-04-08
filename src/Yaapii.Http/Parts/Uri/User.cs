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

using System;
using Yaapii.Atoms;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds the user info part of a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class User : MapInput.Envelope
    {
        private const string KEY = "user";

        /// <summary>
        /// Adds the user info part of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public User(string user) : this(() => user)
        { }

        /// <summary>
        /// Adds the user info part of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public User(IText user) : this(() => user.AsString())
        { }

        /// <summary>
        /// Adds the user info part of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public User(IScalar<string> user) : this(() => user.Value())
        { }

        /// <summary>
        /// Adds the user info part of a <see cref="System.Uri"/> to a request.
        /// </summary>
        public User(Func<string> user) : base(new Kvp.Of(KEY, user))
        { }
    }
}
