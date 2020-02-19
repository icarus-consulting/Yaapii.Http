using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Test
{
    public sealed class ConditionalTests
    {
        [Fact]
        public void DoesNotApplyMapInput()
        {
            Assert.Empty(
                new Conditional(
                    () => false,
                    new MapInput.Of(
                        new Kvp.Of("not empty", "do not apply this input")
                    )
                ).Apply(
                    new Map.Of(new MapInput.Of())
                )
            );
        }

        [Fact]
        public void AppliesMapInput()
        {
            Assert.NotEmpty(
                new Conditional(
                    () => true,
                    new MapInput.Of(
                        new Kvp.Of("some input", "apply this")
                    )
                ).Apply(
                    new Map.Of(new MapInput.Of())
                )
            );
        }
    }
}
