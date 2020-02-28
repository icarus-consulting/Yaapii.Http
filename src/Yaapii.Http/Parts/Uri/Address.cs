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

using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Scalar;

namespace Yaapii.Http.Parts.Uri
{
    /// <summary>
    /// Adds a <see cref="System.Uri"/> to a request.
    /// </summary>
    public sealed partial class Address : MapInput.Envelope
    {
        /// <summary>
        /// Adds a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Address(string uri) : this(new Sticky<System.Uri>(new System.Uri(uri)))
        { }

        /// <summary>
        /// Adds a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Address(System.Uri uri) : this(new ScalarOf<System.Uri>(uri))
        { }

        /// <summary>
        /// Adds a <see cref="System.Uri"/> to a request.
        /// </summary>
        public Address(IScalar<System.Uri> uri) : base(() =>
            new Joined(
                new Scheme(uri.Value().Scheme),
                new Conditional(() =>
                    uri.Value().UserInfo != string.Empty,
                    new User(uri.Value().UserInfo)
                ),
                new Host(uri.Value().Host),
                new Conditional(() =>
                    uri.Value().Port != 0,
                    new Port(uri.Value().Port)
                ),
                new Conditional(() =>
                    uri.Value().AbsolutePath != string.Empty,
                    new Path(uri.Value().AbsolutePath)
                ),
                new Conditional(() =>
                    uri.Value().Query != string.Empty,
                    new QueryParams(uri.Value().Query)
                ),
                new Conditional(() =>
                    uri.Value().Fragment != string.Empty,
                    new Fragment(uri.Value().Fragment)
                )
            )
        )
        { }
    }
}
