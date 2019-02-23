using System;
using System.Net;
using Akka.Actor;

namespace AssistCore.Http
{
    public class ListenerActor : UntypedActor
    {
        private IActorRef _handler;
        private HttpListener _listener = new HttpListener();

        public ListenerActor(IActorRef handler, string prefix)
        {
            _handler = handler;
            _listener.Prefixes.Add(prefix);
        }

        protected override void PreStart()
        {
            _listener.Start();
            _listener.GetContextAsync().PipeTo(Self);
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case HttpListenerContext context:
                    var contextRef = Context.ActorOf(ContextActor.Prop(context.Response));
                    context.ToRequest().PipeTo(_handler, contextRef);
                    _listener.GetContextAsync().PipeTo(Self);
                    break;
            }
        }

        internal static Props Prop(Bind bind)
        {
            return Props.Create(() => new ListenerActor(bind.Handler, bind.Prefix));
        }
    }
}