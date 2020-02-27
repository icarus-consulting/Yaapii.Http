using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class FragmentTests
    {
        [Fact]
        public void WritesFragment()
        {
            Assert.Equal(
                "xyz",
                new Fragment("xyz").Apply(
                    new Map.Of(new MapInput.Of())
                )["fragment"]
            );
        }
    }
}
