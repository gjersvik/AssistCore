using NUnit.Framework;
using AssistCore.Http;
using System;
using System.Collections.Immutable;

namespace AssistCore.Tests.Http
{
    public class RequestTests
    {
        [Test]
        public void Constructor(){
            var req = new Request("GET", new Uri("file://test"),ImmutableDictionary<string,string>.Empty, ImmutableArray<byte>.Empty );
            Assert.Multiple(() => {
                Assert.That(req.Method, Is.EqualTo("GET"), "Method");
                Assert.That(req.Uri, Is.EqualTo(new Uri("file://test")), "Uri");
                Assert.That(req.Headers, Is.EqualTo(ImmutableDictionary<string,string>.Empty), "Headers");
                Assert.That(req.Body, Is.EqualTo(ImmutableArray<byte>.Empty), "Body");
            });
        }
    }
}