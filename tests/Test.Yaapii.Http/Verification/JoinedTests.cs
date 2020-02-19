using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Verification.Test
{
    public sealed class JoinedTests
    {
        [Fact]
        public void VerifiesAll()
        {
            int sum = 0;
            new Joined(
                new Verification(dict =>
                {
                    sum += 1;
                }),
                new Verification(dict =>
                {
                    sum += 2;
                }),
                new Verification(dict =>
                {
                    sum += 4;
                }),
                new Verification(dict =>
                {
                    sum += 8;
                })
            ).Verify(
                new Map.Of(new MapInput.Of())
            );
            Assert.Equal(15, sum);
        }
    }
}
