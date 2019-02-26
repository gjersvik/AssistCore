using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AssistCore.Http
{
    public class Request
    {
        public static Request Get(string uri) => new Request("GET", new Uri(uri));
        public static Request Post(string uri) => new Request("POST", new Uri(uri));

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

        public string BodyToString()
        {
            return Encoding.UTF8.GetString(Body.ToArray());
        }

        public Request SetBody(string text, string mimeType = "text/plain"){
            var headers = Headers.SetItem("Content-Type", $"{mimeType}; charset=utf-8");
            var body = Encoding.UTF8.GetBytes(text).ToImmutableArray();
            return new Request(Method, Uri, headers, body);
        }

        public Request SetBodyForm(IEnumerable<KeyValuePair<string, string>> form)
        {
            var text = string.Join("&", form.Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));
            return SetBody(text, "application/x-www-form-urlencoded");
        }
    }
}
