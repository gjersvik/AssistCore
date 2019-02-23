using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Akka.Actor;

namespace AssistCore.Http
{
    [ExcludeFromCodeCoverage]
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
                    ToRequest(context).PipeTo(_handler, contextRef);
                    _listener.GetContextAsync().PipeTo(Self);
                    break;
            }
        }

        public static Props Prop(Bind bind)
        {
            return Props.Create(() => new ListenerActor(bind.Handler, bind.Prefix));
        }

        private static async Task<Request> ToRequest(HttpListenerContext context)
        {
            var req = context.Request;
            var headers = ImmutableDictionary.CreateBuilder<string, string>();
            foreach (var key in req.Headers.AllKeys)
            {
                headers[key] = req.Headers[key];
            }
            var body = ImmutableArray<byte>.Empty;
            if (req.InputStream != null)
            {
                var ms = new MemoryStream();
                await req.InputStream.CopyToAsync(ms);
                body = ms.ToArray().ToImmutableArray();
            }

            return new Request(req.HttpMethod, req.Url, headers.ToImmutable(), body);
        }
    }
}