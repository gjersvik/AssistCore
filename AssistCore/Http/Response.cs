using System;
using System.Collections.Immutable;

namespace AssistCore.Http
{
    public class Response
    {
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