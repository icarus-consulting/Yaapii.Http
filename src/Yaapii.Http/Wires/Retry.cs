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
using System.Collections.Generic;
using Yaapii.Atoms.Text;
using Yaapii.Atoms.Map;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Wires
{
    /// <summary>
    /// A wire that catches exceptions and retries multiple times to send the request.
    /// Throws an exception if the last attempt fails.
    /// </summary>
    public sealed class Retry : WireEnvelope
    {
        /// <summary>
        /// A wire that catches exceptions and tries up to 3 times to send the request.
        /// Throws an exception if the third attempt fails.
        /// </summary>
        public Retry(IWire origin) : this(3, origin)
        { }

        /// <summary>
        /// A wire that catches exceptions and retries multiple times to send the request.
        /// Throws an exception if the last attempt fails.
        /// </summary>
        public Retry(int attempts, IWire origin) : this(
            attempts,
            (req, ex) =>
                new ApplicationException(
                    new Formatted(
                        "Failed to send {0}-request to '{1}' after {2} attempts.",
                        new Method.Of(req).AsString(),
                        new Address.Of(req).Value().AbsoluteUri,
                        new TextOf(attempts).AsString()
                    ).AsString(),
                    ex
                ),
            origin
        )
        { }

        /// <summary>
        /// A wire that catches exceptions and retries multiple times to send the request.
        /// Throws an exception created from the given funciton if the last attempt fails.
        /// </summary>
        public Retry(int attempts, Func<IDictionary<string, string>, Exception, Exception> exception, IWire origin) : base(request =>
        {
            IDictionary<string, string> response = new MapOf(new MapInputOf());
            for(int i = 1; i <= attempts; i++)
            {
                try
                {
                    response = origin.Response(request);
                }
                catch (Exception ex)
                {
                    if(i == attempts)
                    {
                        throw exception(request, ex);
                    }
                }
            }
            return response;
        })
        { }
    }
}
