using System;
using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Verification.Test
{
    public sealed class VerificationTests
    {
        [Fact]
        public void ThrowsGivenException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Verification(
                    dict => false,
                    dict => new ArgumentException()
                ).Verify(new Map.Of(new MapInput.Of()))
            );
        }
    }
}
