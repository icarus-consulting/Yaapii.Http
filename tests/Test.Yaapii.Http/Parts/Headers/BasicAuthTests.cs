﻿using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class BasicAuthTests
    {
        [Fact]
        public void WritesHeader()
        {
            Assert.Equal(
                $"Basic {new TextBase64("user:password").AsString()}",
                new BasicAuth("user", "password").Apply(
                    new Map.Of(new MapInput.Of())
                )["header:0:Authorization"]
            );
        }
    }
}
