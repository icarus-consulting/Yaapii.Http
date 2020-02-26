﻿using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class UserTests
    {
        [Fact]
        public void WritesUser()
        {
            Assert.Equal(
                "yourNameHere",
                new User("yourNameHere").Apply(
                    new Map.Of(new MapInput.Of())
                )["user"]
            );
        }
    }
}