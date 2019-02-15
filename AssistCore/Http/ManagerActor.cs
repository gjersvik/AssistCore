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

        private async Task<Response> Send(Request req){
            var httpReq = new HttpRequestMessage(new HttpMethod(req.Method), req.Uri);
            if(!req.Body.IsDefaultOrEmpty){
                httpReq.Content = new ByteArrayContent(req.Body.ToArray());
            }
            
            foreach (var kv in req.Headers)
            {
                httpReq.Headers.Remove(kv.Key);
                httpReq.Headers.Add(kv.Key, kv.Value);
            }

            var httpRes = await _client.SendAsync(httpReq);
            var headers = httpRes.Headers.AsEnumerable().Select(kv => new KeyValuePair<string,string>(kv.Key, kv.Value.First())).ToImmutableDictionary();
            var body = (await httpRes.Content.ReadAsByteArrayAsync()).ToImmutableArray();
            return new Response((ushort)httpRes.StatusCode, httpRes.ReasonPhrase, headers, body);
        }
    }
}