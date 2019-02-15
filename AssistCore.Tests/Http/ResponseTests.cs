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
    }
}