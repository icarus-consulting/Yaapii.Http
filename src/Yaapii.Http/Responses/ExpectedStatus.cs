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
using System.Collections.Generic;
using Yaapii.Atoms.Error;
using Yaapii.Http.Verifications;

namespace Yaapii.Http.Responses
{
    /// <summary>
    /// Verifies that a response has a specific status.
    /// </summary>
    public sealed class ExpectedStatus : VerificationEnvelope
    {
        /// <summary>
        /// Verifies that a response has a specific status.
        /// </summary>
        public ExpectedStatus(int status) : this(
            status,
            res => new ApplicationException($"Expected status {status}, but server responded with status {new Status.Of(res).AsInt()}.")
        )
        { }

        /// <summary>
        /// Verifies that a response has a specific status.
        /// </summary>
        public ExpectedStatus(int status, Exception exception) : this(status, res => exception)
        { }

        /// <summary>
        /// Verifies that a response has a specific status.
        /// </summary>
        public ExpectedStatus(int status, Func<IDictionary<string, string>, Exception> exception) : base(response =>
        {
            if (new Status.Of(response).AsInt() != status)
            {
                throw exception(response);
            }
        })
        { }
    }
}
