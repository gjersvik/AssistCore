using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Akka.Actor;

namespace AssistCore.Http
{
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
            }
        }

        private async Task<Response> Send(Request req)
        {
            var res = await _client.SendAsync(req.ToHttpClient());
            return await res.ToMessageAsync();
        }
    }
}