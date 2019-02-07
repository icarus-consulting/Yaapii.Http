using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Map;
using Yaapii.Http;
using Yaapii.Http.Method;
using Yaapii.Http.Mock;

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
