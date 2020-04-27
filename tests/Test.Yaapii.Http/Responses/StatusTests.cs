﻿using Xunit;
using Yaapii.Atoms.Map;

namespace Yaapii.Http.Responses.Test
{
    public sealed class StatusTests
    {
        [Fact]
        public void WritesReason()
        {
            Assert.Equal(
                "200",
                new Status(200).Apply(
                    new MapOf(new MapInputOf())
                )["status"]
            );
        }
    }
}
