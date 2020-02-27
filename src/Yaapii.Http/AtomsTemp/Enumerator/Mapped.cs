﻿// MIT License
//
// Copyright(c) 2019 ICARUS Consulting GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Yaapii.Http.AtomsTemp.Func;

#pragma warning disable NoProperties // No Properties
#pragma warning disable CS1591

namespace Yaapii.Http.AtomsTemp.Enumerator
{
    /// <summary>
    /// Mapped content of an <see cref="IEnumerable{T}"/> to another type using the given function.
    /// </summary>
    /// <typeparam name="In">type of items</typeparam>
    /// <typeparam name="Out">type of mapped items</typeparam>
    public sealed class Mapped<In, Out> : IEnumerator<Out>
    {
        private readonly IEnumerator<In> _enumerator;
        private readonly Func<In, int, Out> _func;
        private readonly List<int> _index;

        /// <summary>
        /// Mapped content of an <see cref="IEnumerable{T}"/> to another type using the given <see cref="IBiFunc{In, Index, Out}"/> function with index.
        /// </summary>
        /// <param name="src">source enumerable</param>
        /// <param name="fnc">mapping function</param>
        public Mapped(IEnumerator<In> src, IFunc<In, Out> fnc) : this(src,
            (input, index) => fnc.Invoke(input))
        { }

        /// <summary>
        /// Mapped content of an <see cref="IEnumerable{T}"/> to another type using the given <see cref="Func{In, Index, Out}"/> function with index.
        /// </summary>
        /// <param name="src">source enumerable</param>
        /// <param name="fnc">mapping function</param>
        public Mapped(IEnumerator<In> src, Func<In, Out> fnc) : this(src, (input, index)=> fnc.Invoke(input))
        { }

        /// <summary>
        /// Mapped content of an <see cref="IEnumerable{T}"/> to another type using the given <see cref="IBiFunc{In, Index, Out}"/> function with index.
        /// </summary>
        /// <param name="src">source enumerable</param>
        /// <param name="fnc">mapping function</param>
        public Mapped(IEnumerator<In> src, IBiFunc<In, int, Out> fnc) : this(src,
        (input, index) => fnc.Invoke(input, index))
        { }

        /// <summary>
        /// Mapped content of an <see cref="IEnumerable{T}"/> to another type using the given <see cref="Func{In, Index, Out}"/> function with index.
        /// </summary>
        /// <param name="src">source enumerable</param>
        /// <param name="fnc">mapping function</param>
        public Mapped(IEnumerator<In> src, Func<In, int, Out> fnc)
        {
            this._enumerator = src;
            this._func = fnc;
            _index = new List<int>() { -1 };
        }

        public Boolean MoveNext()
        {
            var result = this._enumerator.MoveNext();

            if (result)
            {
                _index[0] += 1;
            }

            return result;
        }

        public Out Current
        {
            get
            {
                return this._func.Invoke(this._enumerator.Current, _index[0]);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Reset()
        {
            _index[0] = -1;
            this._enumerator.Reset();
        }

        public void Dispose()
        { }
    }
}
#pragma warning restore NoProperties // No Properties