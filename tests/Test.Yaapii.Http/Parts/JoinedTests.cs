using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Http.Fake;
using Yaapii.Http.Parts.Headers;
using Yaapii.Http.Requests;

namespace Yaapii.Http.Parts.Test
{
    public sealed class JoinedTests
    {
        [Fact]
        public void AppliesAll()
        {
            int sum = 0;
            new Joined(
                new FkPart(dict =>
                {
                    sum += 1;
                    return dict;
                }),
                new FkPart(dict =>
                {
                    sum += 2;
                    return dict;
                }),
                new FkPart(dict =>
                {
                    sum += 4;
                    return dict;
                }),
                new FkPart(dict =>
                {
                    sum += 8;
                    return dict;
                })
            ).Apply(
                new Map.Of(new MapInput.Of())
            ).GetEnumerator();
            Assert.Equal(15, sum);
        }

        [Theory]
        [InlineData(0, "first value")]
        [InlineData(1, "second value")]
        public void DoesNotOverwriteHeaderValues(int index, string expected)
        {
            Assert.Equal(
                expected,
                new Request(
                    new Joined(
                        new Header("same key", "first value")
                    ),
                    new Joined(
                        new Header("same key", "second value")
                    )
                )[$"header:{index}:same key"]
            );
        }
    }
}
