using System;
using System.Collections.Immutable;
using System.Text;
using Newtonsoft.Json;

namespace AssistCore.Http
{
    public class Response
    {
        public static Response Text(string text, string mime_type = "text/plain"){
            var headers = ImmutableDictionary<string,string>.Empty;
            headers = headers.Add("Content-Type", $"{mime_type}; charset=utf-8");

            return new Response(200, "OK",headers, Encoding.UTF8.GetBytes(text).ToImmutableArray() );
        }

        public static Response Html(string html) => Text(html, "text/html");
        public static object Json(object json) => Text(JsonConvert.SerializeObject(json), "application/json");

        public static readonly Response GatewayTimeout = new Response(504, "Gateway Timeout");
        public static readonly Response NotFound = new Response(404, "Not Found");

        public readonly ushort StatusCode;
        public readonly string Reason;
        public readonly ImmutableDictionary<string,string> Headers;
        public readonly ImmutableArray<byte> Body;

        public Response(ushort statusCode, string reason, ImmutableDictionary<string,string> headers = null, ImmutableArray<byte> body = default(ImmutableArray<byte>)){
            StatusCode = statusCode;
            Reason = reason;
            Headers = headers ?? ImmutableDictionary<string,string>.Empty;
            Body = body;
        }
    }
}