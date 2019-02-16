using System;
using System.Collections.Immutable;

namespace AssistCore.Http
{
    public class Request
    {
        public static Request Get(string uri) => new Request("GET", new Uri(uri), ImmutableDictionary<string, string>.Empty, ImmutableArray<byte>.Empty);

        public readonly string Method;
        public readonly Uri Uri;
        public readonly ImmutableDictionary<string, string> Headers;
        public readonly ImmutableArray<byte> Body;

        public Request(string method, Uri uri, ImmutableDictionary<string, string> headers, ImmutableArray<byte> body)
        {
            Method = method;
            Uri = uri;
            Headers = headers;
            Body = body;
        }
    }
}
