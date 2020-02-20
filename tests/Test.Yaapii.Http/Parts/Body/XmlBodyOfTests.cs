using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class XmlBodyOfTests
    {
        [Fact(Skip = "current version of Yaapii.Xml is incompatible with current version of Yaapii.Atoms (20.02.2020)")]
        public void ReadsBody()
        {
            Assert.Equal(
                "<importantData />",
                new XmlBody.Of(
                    new Map.Of("body", "<importantData />")
                ).AsNode().ToString()
            );
        }
    }
}
