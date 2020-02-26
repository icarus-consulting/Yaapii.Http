﻿using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class BearerTokenAuthOfTests
    {
        [Fact]
        public void ReadsHeader()
        {
            Assert.Equal(
                "this is a token",
                new FirstOf<string>(
                    new BearerTokenAuth.Of(
                        new Map.Of("header:0:Authorization", $"Bearer this is a token")
                    )
                ).Value()
            );
        }
    }
}