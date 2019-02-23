using System;
using Akka.TestKit.NUnit3;
using AssistCore.Http;
using NUnit.Framework;

namespace AssistCore.Tests.Http
{
    public class BindTests : TestKit
    {
        [Test]
        public void BindCreate()
        {
            var actorRef = CreateTestProbe();
            var bind = new Bind(actorRef, "http://localhost/");
            Assert.Multiple(() =>
            {
                Assert.That(bind.Handler, Is.EqualTo(actorRef));
                Assert.That(bind.Prefix, Is.EqualTo("http://localhost/"));
            });
        }

        [Test]
        public void BindPort()
        {
            var actorRef = CreateTestProbe();
            var bind = Bind.Port(actorRef, 8080);
            Assert.That(bind.Prefix, Is.EqualTo("http://localhost:8080/"));
        }
    }
}
