using System;
using System.Linq;
using System.Net;
using Akka.Actor;

namespace AssistCore.Http
{
    public class ContextActor : UntypedActor
    {
        private readonly HttpListenerResponse _res;

        public ContextActor(HttpListenerResponse res)
        {
            _res = res;
            this.SetReceiveTimeout(TimeSpan.FromSeconds(10));
        }

        protected override void OnReceive(object message)
        {
            
            switch (message)
            {
                case Response res:
                    _res.StatusCode = res.StatusCode;
                    _res.StatusDescription = res.Reason;
                    foreach (var kv in res.Headers)
                    {
                        _res.Headers.Set(kv.Key, kv.Value);
                    }
                    if(res.Body.IsDefaultOrEmpty){
                        _res.Close();
                    }else{
                        _res.Close(res.Body.ToArray(), false);
                    }
                    Context.Stop(Self);
                    break;
                case ReceiveTimeout timeout:
                    Self.Tell(Response.GatewayTimeout);
                    break;
            }
        }

        internal static Props Prop(HttpListenerResponse response)
        {
            return Props.Create(() => new ContextActor(response));
        }
    }
}