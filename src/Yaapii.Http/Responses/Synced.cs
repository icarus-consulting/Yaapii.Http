﻿//MIT License

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
using System.Threading.Tasks;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Responses
{
    /// <summary>
    /// The result of a response task.
    /// In case an AggregateException with a single inner exception is thrown from the task,
    /// this inner exception is thrown.
    /// </summary>
    public sealed class Synced : MapEnvelope
    {
        /// <summary>
        /// The result of a response task. 
        /// In case an AggregateException with a single inner exception is thrown from the task,
        /// this inner exception is thrown.
        /// </summary>
        public Synced(Task<IDictionary<string, string>> response) : base(() =>
            {
                try
                {
                    return response.Result;
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerExceptions.Count == 1)
                    {
                        throw ex.InnerExceptions[0];
                    }
                    else
                    {
                        throw;
                    }
                }
            },
            live:false
        )
        { }
    }
}
