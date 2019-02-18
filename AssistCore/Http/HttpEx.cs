using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AssistCore.Http
{
    public static class HttpEx
    {
        public static HttpRequestMessage ToHttpClient(this Request req)
        {
            var httpReq = new HttpRequestMessage(new HttpMethod(req.Method), req.Uri);
            if (!req.Body.IsDefaultOrEmpty)
            {
                httpReq.Content = new ByteArrayContent(req.Body.ToArray());
            }

            foreach (var kv in req.Headers)
            {
                httpReq.Headers.Remove(kv.Key);
                httpReq.Headers.Add(kv.Key, kv.Value);
            }
            return httpReq;
        }

        public static async Task<Response> ToMessageAsync(this HttpResponseMessage res)
        {
            var headers = res.Headers.AsEnumerable().Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.First())).ToImmutableDictionary();
            var body = (await res.Content.ReadAsByteArrayAsync()).ToImmutableArray();
            return new Response((ushort)res.StatusCode, res.ReasonPhrase, headers, body);
        }

        public static async Task<Request> ToRequest(this HttpListenerContext context){
            var req = context.Request;
            var headers = ImmutableDictionary.CreateBuilder<string, string>();
            foreach (var key in req.Headers.AllKeys)
            {
                foreach(var value in req.Headers.GetValues(key)){
                    headers[key] = value;
                }
            }
            var body = ImmutableArray<byte>.Empty;
            if(req.InputStream != null){
                var ms = new MemoryStream();
                await req.InputStream.CopyToAsync(ms);
                body = ms.ToArray().ToImmutableArray();
            }

            return new Request(req.HttpMethod,req.Url, headers.ToImmutable(), body );
        }
    }
}
