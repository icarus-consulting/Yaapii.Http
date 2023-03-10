//MIT License

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

using Newtonsoft.Json.Linq;
using Yaapii.Atoms.Map;
using Yaapii.Http.Parts.Bodies;
using Yaapii.JSON;

namespace Yaapii.Http.Responses
{
    public sealed partial class JsonResponse
    {
        /// <summary>
        /// A Response containing the given status, reason and a body from the given json.
        /// </summary>
        public sealed class Of : MessageEnvelope
        {
            /// <summary>
            /// A 200/OK Response containing the given json.
            /// </summary>
            public Of(JToken body, params IMessageInput[] extraParts) : this(
                200,
                "OK",
                body,
                extraParts
            )
            { }

            /// <summary>
            /// A 200/OK Response containing the given json.
            /// </summary>
            public Of(IJSON body, params IMessageInput[] extraParts) : this(
                200,
                "OK",
                body,
                extraParts
            )
            { }

            /// <summary>
            /// A Response containing the given status, reason and a body from the given json.
            /// </summary>
            public Of(int status, string reason, JToken body, params IMessageInput[] extraParts) : this(
                status,
                reason,
                new JSONOf(body),
                extraParts
            )
            { }

            /// <summary>
            /// A Response containing the given status, reason and a body from the given json.
            /// </summary>
            public Of(int status, string reason, IJSON body, params IMessageInput[] extraParts) : base(
                new Response.Of(
                    new Status(status),
                    new Reason(reason),
                    new Body(body),
                    new Parts.Joined(extraParts)
                )
            )
            { }
        }
    }
}
