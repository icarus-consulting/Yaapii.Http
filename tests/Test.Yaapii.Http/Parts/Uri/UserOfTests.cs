using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class UserOfTests
    {
        [Fact]
        public void ReadsUser()
        {
            Assert.Equal(
                "yourNameHere",
                new User.Of(
                    new Map.Of("user", "yourNameHere")
                ).AsString()
            );
        }
    }
}
