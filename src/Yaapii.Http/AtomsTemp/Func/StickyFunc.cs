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
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Http.AtomsTemp.Func
{
    /// <summary>
    /// Func that caches the result and returns from cache.
    /// </summary>
    /// <typeparam name="In">type of input</typeparam>
    /// <typeparam name="Out">type of output</typeparam>
    public sealed class StickyFunc<In, Out> : IFunc<In, Out>
    {
        /// <summary>
        /// original func
        /// </summary>
        private readonly IFunc<In, Out> _func;

        /// <summary>
        /// cache
        /// </summary>
        private readonly Dictionary<In, Out> _cache;

        /// <summary>
        /// Reload Condition Func
        /// </summary>
        private readonly IFunc<Out,bool> _reloadConditionFnc;

        /// <summary>
        /// Func that caches the result and returns from cache.
        /// </summary>
        /// <param name="fnc">func to cache output from</param>
        public StickyFunc(System.Func<In, Out> fnc) : 
            this(new FuncOf<In, Out>((X) => fnc(X)))
        { }

        /// <summary>
        /// Func that caches the result and returns from cache.
        /// </summary>
        /// <param name="fnc">func to cache output from</param>
        public StickyFunc(IFunc<In, Out> fnc) : this(fnc, new Func<Out, bool>(input=>false))
        { }

        /// <summary>
        /// Func that caches the result and returns from cache with reload condition func
        /// </summary>
        /// <param name="fnc">func to cache output from</param>
        /// <param name="reloadConditionFnc">reload condition func</param>
        public StickyFunc(Func<In, Out> fnc, Func<Out, bool> reloadConditionFnc) : this(new FuncOf<In, Out>((X) => fnc(X)), new FuncOf<Out, bool>((output) => reloadConditionFnc(output)))
        { }

        /// <summary>
        /// Func that caches the result and returns from cache with reload condition func
        /// </summary>
        /// <param name="fnc">func to cache output from</param>
        /// <param name="reloadConditionFnc">reload condition func</param>
        public StickyFunc(IFunc<In, Out> fnc, Func<Out, bool> reloadConditionFnc) : this(fnc, new FuncOf<Out,bool> ((output)=>reloadConditionFnc(output)))
        { }

        /// <summary>
        /// Func that caches the result and returns from cache with reload condition func
        /// </summary>
        /// <param name="fnc">func to cache output from</param>
        /// <param name="reloadConditionFnc">reload condition func</param>
        public StickyFunc(Func<In, Out> fnc, IFunc<Out, bool> reloadConditionFnc) : this(new FuncOf<In, Out>((X) => fnc(X)), reloadConditionFnc)
        { }


        /// <summary>
        /// Func that caches the result and returns from cache with reload condition func
        /// </summary>
        /// <param name="fnc">func to cache output from</param>
        /// <param name="reloadConditionFnc">reload condition func</param>
        public StickyFunc(IFunc<In, Out> fnc, IFunc<Out, bool> reloadConditionFnc)
        {
            this._func = fnc;
            this._cache = new Dictionary<In, Out>();
            _reloadConditionFnc = reloadConditionFnc;
        }

        /// <summary>
        /// Invoke the function and retrieve the output.
        /// </summary>
        /// <param name="input">input argument</param>
        /// <returns>output</returns>
        public Out Invoke(In input)
        {
            if (!this._cache.ContainsKey(input) || _reloadConditionFnc.Invoke(_cache[input]))
            {
                this._cache[input] = _func.Invoke(input);
            }
            return this._cache[input];
        }

    }
}
