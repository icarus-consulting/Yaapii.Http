using Xunit;
using Yaapii.Http.AtomsTemp.Lookup;

namespace Yaapii.Http.Parts.Bodies.Test
{
    public sealed class XmlBodyOfTests
    {
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
