using System;
using Akka.Actor;

namespace AssistCore.Http
{
    public class Bind
    {
        public static Bind Port(IActorRef handler, ushort port)
        {
            return new Bind(handler, $"http://localhost:{port}/");
        }
        public readonly IActorRef Handler;
        public readonly string Prefix;

        public Bind(IActorRef handler, string prefix)
        {
            Handler = handler;
            Prefix = prefix;
        }
    }
}
