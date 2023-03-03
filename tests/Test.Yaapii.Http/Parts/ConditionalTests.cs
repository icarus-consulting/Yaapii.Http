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

using Xunit;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Parts.Test
{
    public sealed class ConditionalTests
    {
        [Fact]
        public void DoesNotApplyMapInput()
        {
            Assert.Empty(
                new Conditional(
                    () => false,
                    new SimpleMessageInput(
                        new KvpOf("not empty", "do not apply this input")
                    )
                ).Apply(
                    new SimpleMessage()
                ).Head().Keys
            );
        }

        [Fact]
        public void AppliesMapInput()
        {
            Assert.NotEmpty(
                new Conditional(
                    () => true,
                    new SimpleMessageInput(
                        new KvpOf("some input", "apply this")
                    )
                ).Apply(
                    new SimpleMessage()
                ).Head().Keys
            );
        }
    }
}
