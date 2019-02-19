using System.Collections.Generic;
using Akka.Actor;

namespace AssistCore.Http
{
    public class ExactPathRouter : UntypedActor
    {
        public class Register
        {
            public readonly string Path;
            public readonly IActorRef Handler;

            public Register(string path, IActorRef handler){
                Path = path;
                Handler = handler;
            }
        }
        private Dictionary<string,IActorRef> _handlers = new Dictionary<string, IActorRef>();

        protected override void OnReceive(object message)
        {
            
            switch (message)
            {
                case Register reg:
                    _handlers[reg.Path] = reg.Handler;
                    break;
                case Request req:
                    if(_handlers.ContainsKey(req.Uri.AbsolutePath)){
                        _handlers[req.Uri.AbsolutePath].Tell(req, Sender);
                    }else{
                        Sender.Tell(Response.NotFound);
                    }
                    Self.Tell(Response.GatewayTimeout);
                    break;
            }
        }
    }
}