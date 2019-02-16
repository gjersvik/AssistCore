using System;
using AssistCore.Http;
using NUnit.Framework;
using System.Collections.Immutable;
using System.Net.Http;

namespace AssistCore.Tests.Http
{
    public class HttpExTests
    {
        [Test]
        public void ToHttpClient()
        {
            var headers = ImmutableDictionary<string, string>.Empty.Add("User-Agent", "Assist Core Tests");
            var body = ImmutableArray<byte>.Empty.Add(42);
            var req = new Request("POST", new Uri("http://example.com/"), headers, body);

            var httpReq = req.ToHttpClient();

            Assert.Multiple(() =>
            {
                Assert.That(httpReq.Method, Is.EqualTo(HttpMethod.Post));
                Assert.That(httpReq.RequestUri, Is.EqualTo(new Uri("http://example.com/")));
                Assert.That(httpReq.Headers.UserAgent.ToString(), Is.EqualTo("Assist Core Tests"));
                Assert.That(httpReq.Content.Headers.ContentLength, Is.EqualTo(1));
            });
        }
    }
}
