﻿using Xunit;
using Yaapii.Atoms.Scalar;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Request;

namespace Yaapii.Http.Wire.Test
{
    public sealed class RefinedTests
    {
        [Fact]
        public void AddsRequestParts()
        {
            var hasToken = false;
            new Refined(
                new FkWire(req =>
                {
                    if (new FirstOf<string>(new BearerTokenAuth.Of(req)).Value() == "this is a token")
                    {
                        hasToken = true;
                    }
                    return new Response.Response.Of(200, "OK");
                }),
                new BearerTokenAuth("this is a token")
            ).Response(new Get());
            Assert.True(hasToken);
        }
    }
}
