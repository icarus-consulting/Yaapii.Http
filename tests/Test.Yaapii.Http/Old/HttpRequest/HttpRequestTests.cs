using System;
using Xunit;
using Yaapii.Http;
using Yaapii.Http.MethodX;

namespace Test.Yaapii.Http
{
    public sealed class HttpRequestTests
    {
        [Fact]
        public void AcceptsTimeout()
        {
            Assert.True(
                new HttpRequest(
                    new Get(), 
                    new Uri("http://www.github.com"), 
                    new TimeSpan(0, 5, 0)
                ).Response().Status() == 200
            );
        }
    }
}
