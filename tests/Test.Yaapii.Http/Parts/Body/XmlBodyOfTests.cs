using Xunit;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Body.Test
{
    public sealed class XmlBodyOfTests
    {
        [Fact(Skip = "current version of Yaapii.Xml is incompatible with current version of Yaapii.Atoms")]
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
