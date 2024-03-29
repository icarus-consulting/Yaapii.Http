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

using System.Collections.Generic;
using Yaapii.Atoms.Map;
using Yaapii.Http.Parts.Bodies;

namespace Yaapii.Http.Responses
{
    public sealed partial class FormResponse
    {
        /// <summary>
        /// A Response containing the given status, reason and form data.
        /// </summary>
        public sealed class Of : MessageEnvelope
        {
            /// <summary>
            /// A 200/OK Response containing the given form data.
            /// </summary>
            public Of(params string[] pairSequence) : this(
                200,
                "OK",
                new MapOf(pairSequence)
            )
            { }

            /// <summary>
            /// A Response containing the given status, reason and form data.
            /// </summary>
            public Of(int status, string reason, params string[] pairSequence) : this(
                status,
                reason,
                new MapOf(pairSequence)
            )
            { }

            /// <summary>
            /// A 200/OK Response containing the given form data.
            /// </summary>
            public Of(IDictionary<string, string> formParams, params IMessageInput[] extraParts) : this(
                200,
                "OK",
                formParams,
                extraParts
            )
            { }

            /// <summary>
            /// A Response containing the given status, reason and form data.
            /// </summary>
            public Of(int status, string reason, IDictionary<string, string> formParams, params IMessageInput[] extraParts) : base(
                new Response.Of(
                    new Status(status),
                    new Reason(reason),
                    new FormParams(formParams),
                    new Parts.Joined(extraParts)
                )
            )
            { }
        }
    }
}
