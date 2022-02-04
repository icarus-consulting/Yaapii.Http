//MIT License

//Copyright(c) 2021 ICARUS Consulting GmbH

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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Text;
using Yaapii.Http.Parts.Headers;

namespace Yaapii.Http.Parts.Bodies
{
    public sealed partial class TextBody
    {
        private const string KEY = "body";

        /// <summary>
        /// The body of a request or response as <see cref="IText"/>
        /// </summary>
        public sealed class Of : TextEnvelope
        {
            /// <summary>
            /// The body of a request or response as <see cref="IText"/>
            /// </summary>
            public Of(Task<IDictionary<string, string>> input) : this(new Responses.Synced(input))
            { }

            /// <summary>
            /// The body of a request or response as <see cref="IText"/>
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                {
                    var encoding = "utf-8";
                    if(new ContentType.Exists(input).Value())
                    {
                        var captures =
                            new Regex(
                                "charset=(?<encoding>utf-7|utf-8|utf-16|utf-32|us-ascii)"
                            ).Match(
                                new Atoms.Text.Joined("; ",
                                    new ContentType.Of(input),
                                    true
                                ).AsString().ToLower()
                            ).Groups["encoding"].Captures;

                        if(captures.Count > 0)
                        {
                            encoding = captures[0].Value;
                        }
                    }

                    return
                        new TextOf(
                            new Base64Bytes(
                                new BytesOf(
                                    new FallbackMap(
                                        input,
                                        key => throw new InvalidOperationException("Failed to extract body as text. No body found.")
                                    )[TextBody.KEY]
                                )
                            ),
                            new MapOf<Encoding>(
                                new KvpOf<Encoding>("utf-7", Encoding.UTF7),
                                new KvpOf<Encoding>("utf-8", Encoding.UTF8),
                                new KvpOf<Encoding>("utf-16", Encoding.Unicode),
                                new KvpOf<Encoding>("utf-32", Encoding.UTF32),
                                new KvpOf<Encoding>("us-ascii", Encoding.ASCII)
                            )[encoding.ToLower()]
                        ).AsString();
                },
                live: false
            )
            { }
        }
    }
}
