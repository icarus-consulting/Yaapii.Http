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

using System.Collections.Generic;
using Yaapii.Atoms.Text;
using Yaapii.Atoms.Enumerable;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Uri
{
    public sealed partial class Address
    {
        /// <summary>
        /// Extracts the <see cref="System.Uri"/> from a request.
        /// </summary>
        public sealed class Of : UriEnvelope
        {
            /// <summary>
            /// Extracts the <see cref="System.Uri"/> from a request.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new System.Uri(
                    new Formatted("{0}://{1}{2}{3}{4}{5}{6}",
                        new Scheme.Of(input),
                        new OptionalText(
                            new User.Exists(input),
                            () => $"{new User.Of(input).AsString()}@"
                        ),
                        new Host.Of(input),
                        new OptionalText(
                            new Port.Exists(input),
                            () => $":{new Port.Of(input).AsInt()}"
                        ),
                        new OptionalText(
                            new Path.Exists(input),
                            new Path.Of(input)
                        ),
                        new OptionalText<IDictionary<string, string>>(
                            new QueryParams.Of(input),
                            dict => dict.Keys.Count > 0,
                            dict => "?" +
                                new Yaapii.Atoms.Text.Joined("&",
                                    new MappedDictionary<string>((key, value) =>
                                        $"{key}={value()}",
                                        new QueryParams.Of(input)
                                    )
                                ).AsString()
                        ),
                        new OptionalText(
                            new Fragment.Exists(input),
                            () => $"#{new Fragment.Of(input).AsString()}"
                        )
                    ).AsString()
                )
            )
            { }
        }
    }
}
