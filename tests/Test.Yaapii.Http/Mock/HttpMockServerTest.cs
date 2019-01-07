using System.Net.Http;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Yaapii.Http.Mock.Test
{
    [Collection("Yaapii http responses")]
    public sealed class HttpMockTest
    {
        [Fact]
        public void RespondsString()
        {
            var port = 6789;
            using (new HttpMock(port)
                .With("test", (req, res, args) => "hello internet"))
            {
                var client = new HttpClient();
                var response = client.GetStringAsync($"http://localhost:{port}/test").Result;
                Assert.Equal("hello internet", response);
            }
        }

        [Fact]
        public void RepondsWithMultipleUrls()
        {
            var port = 6789;
            using (new HttpMock(port)
                .With("1", (req, res, args) => "hello")
                .With("2", (req, res, args) => "internet"))
            {
                var client = new HttpClient();
                var response =
                    client.GetStringAsync($"http://localhost:{port}/1").Result +
                    " " +
                    client.GetStringAsync($"http://localhost:{port}/2").Result;

                Assert.Equal("hello internet", response);
            }
        }

        [Fact]
        public void RespondsBytes()
        {
            var port = 6789;
            using (new HttpMock(port)
                .With("test", (req, res, args) => new InputOf("hello internet")))
            {
                var client = new HttpClient();
                var response =
                    new TextOf(
                        client.GetByteArrayAsync($"http://localhost:{port}/test").Result
                    ).AsString();
                Assert.Equal("hello internet", response);
            }
        }
    }
}
