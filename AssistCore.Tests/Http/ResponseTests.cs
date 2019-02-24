using NUnit.Framework;
using AssistCore.Http;
using System;
using System.Collections.Immutable;

namespace AssistCore.Tests.Http
{
    public class ResponseTests
    {
        [Test]
        public void Constructor(){
            var res = new Response(200, "OK",ImmutableDictionary<string,string>.Empty, ImmutableArray<byte>.Empty );
            Assert.Multiple(() => {
                Assert.That(res.StatusCode, Is.EqualTo(200), "StatusCode");
                Assert.That(res.Reason, Is.EqualTo("OK"), "Reason");
                Assert.That(res.Headers, Is.EqualTo(ImmutableDictionary<string,string>.Empty), "Headers");
                Assert.That(res.Body, Is.EqualTo(ImmutableArray<byte>.Empty), "Body");
            });
        }
        [Test]
        public void GatewayTimeout(){
            var res = Response.GatewayTimeout;
            Assert.Multiple(() => {
                Assert.That(res.StatusCode, Is.EqualTo(504), "StatusCode");
                Assert.That(res.Reason, Is.EqualTo("Gateway Timeout"), "Reason");
            });
        }
        [Test]
        public void NotFound(){
            var res = Response.NotFound;
            Assert.Multiple(() => {
                Assert.That(res.StatusCode, Is.EqualTo(404), "StatusCode");
                Assert.That(res.Reason, Is.EqualTo("Not Found"), "Reason");
            });
        }
        [Test]
        public void Text(){
            var res = Response.Text("Test");
            Assert.Multiple(() => {
                Assert.That(res.StatusCode, Is.EqualTo(200));
                Assert.That(res.Reason, Is.EqualTo("OK"));
                Assert.That(res.Headers["Content-Type"], Is.EqualTo("text/plain; charset=utf-8"));
                Assert.That(res.Body, Is.EqualTo(new byte[]{84, 101, 115, 116}).AsCollection);
            });
        }
    }
}