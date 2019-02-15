using System;
using Akka.TestKit.NUnit3;
using Akka.Actor;
using AssistCore.Http;
using NUnit.Framework;

namespace AssistCore.Tests.Http
{
    public class ManagerActorTests: TestKit
    {
        [Test]
        public void GetGoogle(){
            var subject = this.Sys.ActorOf<ManagerActor>();

            subject.Tell(Request.Get("http://google.com/"), this.TestActor);

            var res = ExpectMsg<Response>();

            Assert.That(res.Body, Is.Not.Empty);
        }
    }
}
