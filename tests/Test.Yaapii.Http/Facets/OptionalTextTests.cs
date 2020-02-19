using Xunit;

namespace Yaapii.Http.Facets.Test
{
    public sealed class OptionalTextTests
    {
        [Fact]
        public void DefaultsToEmpty()
        {
            Assert.Equal(
                string.Empty,
                new OptionalText(
                    () => false,
                    () => "not empty"
                ).AsString()
            );
        }

        [Fact]
        public void HasText()
        {
            Assert.Equal(
                "some text",
                new OptionalText(
                    () => true,
                    () => "some text"
                ).AsString()
            );
        }
    }
}
