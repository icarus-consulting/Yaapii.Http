﻿using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class HostTests
    {
        [Fact]
        public void WritesHost()
        {
            Assert.Equal(
                "localhost",
                new Host("localhost").Apply(
                    new Map.Of(new MapInput.Of())
                )["host"]
            );
        }
    }
}
