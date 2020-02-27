﻿using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Uri.Test
{
    public sealed class FragmentOfTests
    {
        [Fact]
        public void ReadsFragment()
        {
            Assert.Equal(
                "qwertz",
                new Fragment.Of(
                    new Map.Of("fragment", "qwertz")
                ).AsString()
            );
        }
    }
}
