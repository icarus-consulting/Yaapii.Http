using Xunit;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Parts.Headers.Test
{
    public sealed class AuthorizationOfTests
    {
        [Fact]
        public void ReadsHeader()
        {
            Assert.Equal(
                "Basic dXNlcjpwYXNzd29yZA==",
                new FirstOf<string>(
                    new Authorization.Of(
                        new Map.Of("header:0:Authorization", "Basic dXNlcjpwYXNzd29yZA==")
                    )
                ).Value()
            );
        }
    }
}
