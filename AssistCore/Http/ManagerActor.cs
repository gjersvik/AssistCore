using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Akka.Actor;

namespace AssistCore.Http
{
    [ExcludeFromCodeCoverage]
    public class ManagerActor : UntypedActor
    {
        private readonly HttpClient _client = new HttpClient();

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Request req:
                    Send(req).PipeTo(Sender);
                    break;
                case Bind bind:
                    Context.ActorOf(ListenerActor.Prop(bind));
                    break;
            }
        }

        private async Task<Response> Send(Request req)
        {
            var res = await _client.SendAsync(req.ToHttpClient());
            return await res.ToMessageAsync();
        }
    }
}