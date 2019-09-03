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
using System.Globalization;
using System.IO;
using System.Text;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;

#pragma warning disable MaxClassLength // Class length max
namespace Yaapii.Atoms.Text
{
    /// <summary>
    /// A <see cref="IText"/> out of other objects.
    /// </summary>
    public sealed class StickyText : IText
    {
        private readonly Lazy<String> origin;

        /// <summary>
        /// A <see cref="IText"/> out of a int.
        /// </summary>
        /// <param name="input">number</param>
        public StickyText(int input) : this(() => input + "")
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a long.
        /// </summary>
        /// <param name="input">number</param>
        public StickyText(long input) : this(() => input + "")
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a double
        /// </summary>
        /// <param name="input">a <see cref="double"/></param>
        public StickyText(double input) : this(
            () => input.ToString(CultureInfo.InvariantCulture)
        )
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a double
        /// </summary>
        /// <param name="input">a <see cref="double"/></param>
        /// <param name="cultureInfo">The </param>
        public StickyText(double input, CultureInfo cultureInfo) : this(
            () => input.ToString(cultureInfo)
        )
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a float
        /// </summary>
        /// <param name="input">a <see cref="float"/></param>
        public StickyText(float input) : this(
            () => input.ToString(CultureInfo.InvariantCulture)
        )
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a double
        /// </summary>
        /// <param name="input">a <see cref="float"/></param>
        /// <param name="cultureInfo">The </param>
        public StickyText(float input, CultureInfo cultureInfo) : this(
            () => input.ToString(cultureInfo)
        )
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">a file <see cref="Uri"/></param>
        public StickyText(Uri uri) : this(new InputOf(uri))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">a file <see cref="Uri"/></param>
        public StickyText(Uri uri, Encoding encoding) : this(new InputOf(uri), encoding)
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="FileInfo"/>
        /// </summary>
        /// <param name="file"></param>
        public StickyText(FileInfo file) : this(new InputOf(file))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="FileInfo"/>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        public StickyText(FileInfo file, Encoding encoding) : this(new InputOf(file), encoding)
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="IInput"/>.
        /// </summary>
        /// <param name="input">a <see cref="IInput"/></param>
        public StickyText(IInput input) : this(new BytesOf(input))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="IInput"/>.
        /// </summary>
        /// <param name="input">a input</param>
        /// <param name="max">maximum buffer size</param>
        public StickyText(IInput input, int max) : this(input, max, Encoding.GetEncoding(0))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="IInput"/>.
        /// </summary>
        /// <param name="input">a input</param>
        /// <param name="encoding"><see cref="Encoding"/> of the input</param>
        public StickyText(IInput input, Encoding encoding) : this(new BytesOf(input), encoding)
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="IInput"/>.
        /// </summary>
        /// <param name="input">a <see cref="IInput"/></param>
        /// <param name="encoding">encoding of the <see cref="IInput"/></param>
        /// <param name="max">maximum buffer size</param>
        public StickyText(IInput input, int max, Encoding encoding) : this(new BytesOf(input, max), encoding)
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="StreamReader"/>.
        /// </summary>
        /// <param name="rdr">a <see cref="StreamReader"/></param>
        public StickyText(StringReader rdr) : this(new BytesOf(new InputOf(rdr)))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="StreamReader"/>.
        /// </summary>
        /// <param name="rdr">a <see cref="StreamReader"/></param>
        /// <param name="enc"><see cref="Encoding"/> of the <see cref="StreamReader"/></param>
        public StickyText(StringReader rdr, Encoding enc) : this(new BytesOf(rdr), enc)
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="StreamReader"/>.
        /// </summary>
        /// <param name="rdr">a <see cref="StreamReader"/></param>
        public StickyText(StreamReader rdr) : this(new BytesOf(rdr))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="StreamReader"/>.
        /// </summary>
        /// <param name="rdr">a <see cref="StreamReader"/></param>
        /// <param name="cset"><see cref="Encoding"/> of the <see cref="StreamReader"/></param>
        public StickyText(StreamReader rdr, Encoding cset) : this(new BytesOf(rdr, cset))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="StreamReader"/>.
        /// </summary>
        /// <param name="rdr">a <see cref="StreamReader"/></param>
        /// <param name="cset"><see cref="Encoding"/> of the <see cref="StreamReader"/></param>
        /// <param name="max">maximum buffer size</param>
        public StickyText(StreamReader rdr, Encoding cset, int max) : this(new BytesOf(rdr, cset, max))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="builder">a <see cref="StringBuilder"/></param>
        public StickyText(StringBuilder builder) : this(new BytesOf(builder))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="builder">a <see cref="StringBuilder"/></param>
        /// <param name="enc"><see cref="Encoding"/> of the <see cref="StreamReader"/></param>
        public StickyText(StringBuilder builder, Encoding enc) : this(new BytesOf(builder, enc))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="char"/> array.
        /// </summary>
        /// <param name="chars">a char array</param>
        public StickyText(params char[] chars) : this(new BytesOf(chars))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="char"/> array.
        /// </summary>
        /// <param name="chars">a char array</param>
        /// <param name="encoding"><see cref="Encoding"/> of the chars</param>
        public StickyText(char[] chars, Encoding encoding) : this(new BytesOf(chars, encoding))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="Exception"/>.
        /// </summary>
        /// <param name="error"><see cref="Exception"/> to serialize</param>
        public StickyText(Exception error) : this(new BytesOf(error))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of a <see cref="byte"/> array.
        /// </summary>
        /// <param name="bytes">a byte array</param>
        public StickyText(params byte[] bytes) : this(new BytesOf(bytes))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of <see cref="IBytes"/> object.
        /// </summary>
        /// <param name="bytes">A <see cref="IBytes"/> object</param>
        public StickyText(IBytes bytes) : this(bytes, Encoding.GetEncoding(0))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of <see cref="IBytes"/> object.
        /// </summary>
        /// <param name="bytes">A <see cref="IBytes"/> object</param>
        /// <param name="encoding"><see cref="Encoding"/> of the <see cref="IBytes"/> object</param>
        public StickyText(IBytes bytes, Encoding encoding) : this(
            () =>
            {
                var memoryStream = new MemoryStream(bytes.AsBytes());
                return new StreamReader(memoryStream, encoding).ReadToEnd(); // removes the BOM from the Byte-Array
            })
        { }

        /// <summary>
        /// A <see cref="IText"/> out of <see cref="string"/>.
        /// </summary>
        /// <param name="input">a string</param>
        public StickyText(String input) : this(input, Encoding.GetEncoding(0))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of <see cref="string"/>.
        /// </summary>
        /// <param name="input">a string</param>
        /// <param name="encoding"><see cref="Encoding"/> of the string</param>
        public StickyText(String input, Encoding encoding) : this(
            () => encoding.GetString(encoding.GetBytes(input)))
        { }

        /// <summary>
        /// A <see cref="IText"/> out of the return value of a <see cref="IFunc{T}"/>.
        /// </summary>
        /// <param name="fnc">func returning a string</param>
        public StickyText(IFunc<string> fnc) : this(() => fnc.Invoke())
        { }

        /// <summary>
        /// A <see cref="IText"/> out of encapsulating <see cref="IScalar{T}"/>.
        /// </summary>
        /// <param name="scalar">scalar of a string</param>
        public StickyText(Func<String> scalar)
        {
            this.origin = new Lazy<string>(() => scalar());
        }

        /// <summary>
        /// Gives the text as a string.
        /// </summary>
        /// <returns></returns>
        public String AsString()
        {
            return this.origin.Value;
        }

        /// <summary>
        /// Compares to another text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int CompareTo(IText text)
        {
            return this.AsString().CompareTo(text.AsString());
        }

        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj as IText == null) return false;
            return this.AsString().CompareTo((obj as IText).AsString()) == 0;
        }

        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool Equals(IText text)
        {
            return Equals(text as object);
        }

        /// <summary>
        /// Hashcode for this text
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}