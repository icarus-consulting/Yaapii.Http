// MIT License
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

namespace Yaapii.Atoms.Enumerable
{
    /// <summary>
    /// An enumerable.
    /// Difference to Atoms.EnumerableOf: it internally uses C# List which is sticky and faster.
    /// </summary>
    public class Many<T> : IEnumerable<T>
    {
        private readonly List<T> items;

        /// <summary>
        /// An enumerable.
        /// </summary>
        public Many(params Func<T>[] buildItems) : this(() => {
            var lst = new List<T>();
            foreach (var buildItem in buildItems)
            {
                lst.Add(buildItem());
            }
            return lst;
        })
        { }

        /// <summary>
        /// An enumerable.
        /// </summary>
        public Many(params T[] items) : this(() => new List<T>(items))
        { }

        /// <summary>
        /// An enumerable.
        /// </summary>
        public Many(IEnumerator<T> items) : this(() => new List<T>(new EnumerableOf<T>(items)))
        { }

        /// <summary>
        /// An enumerable.
        /// </summary>
        public Many(IEnumerable<T> src) : this(() => src)
        { }

        /// <summary>
        /// An enumerable.
        /// </summary>
        public Many(Func<IEnumerable<T>> src)
        {
            this.items = new List<T>(src());
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
