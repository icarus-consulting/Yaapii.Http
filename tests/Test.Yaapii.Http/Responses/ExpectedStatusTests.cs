using System;
using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Responses.Test
{
    public sealed class ExpectedStatusTests
    {
        [Fact]
        public void RejectsWrongStatus()
        {
            Assert.Throws<ArgumentException>(() =>
                new ExpectedStatus(200, new ArgumentException()).Verify(
                    new Map.Of(
                        new Status(400)
                    )
                )
            );
        }

        [Fact]
        public void AcceptsCorrectStatus()
        {
            new ExpectedStatus(200, new ArgumentException()).Verify(
                new Map.Of(
                    new Status(200)
                )
            );
        }
    }
}
