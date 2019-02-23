using System;
using System.Collections.Immutable;

namespace AssistCore.Http
{
    public class Request
    {
        public static Request Get(string uri) => new Request("GET", new Uri(uri));

        public readonly string Method;
        public readonly Uri Uri;
        public readonly ImmutableDictionary<string, string> Headers;
        public readonly ImmutableArray<byte> Body;

        public Request(string method, Uri uri, ImmutableDictionary<string, string> headers = null, ImmutableArray<byte> body = default(ImmutableArray<byte>))
        {
            Method = method;
            Uri = uri;
            Headers = headers ?? ImmutableDictionary<string, string>.Empty;
            Body = body;
        }

        public Request SetHeader(string key, string value)
        {
            var headers = Headers.SetItem(key, value);
            return new Request(Method, Uri, headers, Body);
        }
    }
}
