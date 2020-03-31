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
using Yaapii.Atoms;
using Yaapii.Atoms.Error;

namespace Yaapii.Http.Verifications
{
    /// <summary>
    /// Verifies that a request or response fulfills a given condition.
    /// </summary>
    public sealed class Verification : VerificationEnvelope
    {
        /// <summary>
        /// Verifies that a request or response fulfills a given condition.
        /// </summary>
        public Verification(Func<IDictionary<string, string>, bool> isValid, Func<IDictionary<string, string>, Exception> error) : this(input =>
            new FailWhen(
                !isValid(input),
                error(input)
            )
        )
        { }

        /// <summary>
        /// Verifies that a request or response fulfills a given condition.
        /// </summary>
        public Verification(Func<IDictionary<string, string>, IFail> verify) : this(input => verify(input).Go())
        { }

        /// <summary>
        /// Verifies a request or response.
        /// </summary>
        public Verification(Action<IDictionary<string, string>> verify) : base(verify)
        { }
    }
}
