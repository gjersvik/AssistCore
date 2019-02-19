using System;
using System.Collections.Immutable;
using System.Text;

namespace AssistCore.Http
{
    public class Response
    {
        public static Response Text(string text){
            var headers = ImmutableDictionary<string,string>.Empty;
            headers = headers.Add("Content-Type","text/plain; charset=utf-8");

            return new Response(200, "OK",headers, Encoding.UTF8.GetBytes(text).ToImmutableArray() );
        }

        public readonly ushort StatusCode;
        public readonly string Reason;
        public readonly ImmutableDictionary<string,string> Headers;
        public readonly ImmutableArray<byte> Body;

        public Response(ushort statusCode, string reason, ImmutableDictionary<string,string> headers, ImmutableArray<byte> body){
            StatusCode = statusCode;
            Reason = reason;
            Headers = headers;
            Body = body;
        }
    }
}