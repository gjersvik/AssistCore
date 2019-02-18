using System;
using Akka.Actor;

namespace AssistCore.Http
{
    public class Bind
    {
        public readonly IActorRef Handeler;
        public readonly string Prefix;

        public Bind(IActorRef handeler, string prefix)
        {
            Handeler = handeler;
            Prefix = prefix;
        }
    }
}
