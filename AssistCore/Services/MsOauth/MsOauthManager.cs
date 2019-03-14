using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Akka.Actor;
using AssistCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace AssistCore.Services.MsOAuth
{
    
    public class OAuthMs
    {
        private readonly MsOauthConfig _config;

        public OAuthMs(MsOauthConfig config){
            _config = config;
        }

        public Uri AuthorizeUri(){
            var uri = $"https://login.microsoftonline.com/{_config.TenantId}/oauth2/v2.0/authorize";
            var query = new Dictionary<string, string>
            {
                { "client_id", _config.ClientId },
                { "response_type", "code" },
                { "redirect_uri", _config.RedirectUri },
                { "scope", "User.Read offline_access" },
                { "response_mode", "form_post" }
            };

            uri = QueryHelpers.AddQueryString(uri, query);
            return new Uri(uri);
        }

        public Request NewTokenMessage(string code){
            var req = Request.Post($"https://login.microsoftonline.com/{_config.TenantId}/oauth2/v2.0/token");
            return req.SetBodyForm(new Dictionary<string, string>
            {
                { "client_id", _config.ClientId },
                { "grant_type", "authorization_code" },
                { "scope", "User.Read offline_access" },
                { "code", code },
                { "redirect_uri", _config.RedirectUri },
                { "client_secret", _config.Secret},
            });
        }

        public Request RefreshTokenMessage(string refreshToken){
            var req = Request.Post($"https://login.microsoftonline.com/{_config.TenantId}/oauth2/v2.0/token");
            return req.SetBodyForm(new Dictionary<string, string>
            {
                { "client_id", _config.ClientId },
                { "grant_type", "refresh_token" },
                { "scope", "User.Read offline_access" },
                { "refresh_token", refreshToken },
                { "redirect_uri", _config.RedirectUri },
                { "client_secret", _config.Secret},
            });
        }
    }


    public sealed class PostCode
    {
        public readonly string Code;
        public readonly string State;
        public PostCode(string formData)
        {
            var reader = new FormReader(formData);
            var form = reader.ReadForm().ToImmutableDictionary();
            Code = form.GetValueOrDefault("code").FirstOrDefault();
            State = form.GetValueOrDefault("state").FirstOrDefault();
        }
    };

    public sealed class PostToken
    {
        public readonly string RefreshToken;
        public PostToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }

    public sealed class GetToken
    {

    }

    public class OAuthMsActor : UntypedActor
    {
        private JwtSecurityToken _token;
        private string _refresh_token = "";
        private List<ICanTell> _waitingForToken = new List<ICanTell>();

        private IActorRef _http;
        private OAuthMs _auth;

        public OAuthMsActor(IActorRef http, OAuthMs auth){
            _http = http;
            _auth = auth;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case PostCode code:
                    _http.Tell(_auth.NewTokenMessage(code.Code),Self);
                    Self.Tell(new GetToken(), Sender);
                    break;
                case PostToken token:
                    _refresh_token = token.RefreshToken;
                    _http.Tell(_auth.RefreshTokenMessage(_refresh_token),Self);
                    Self.Tell(new GetToken(), Sender);
                    break;
                case GetToken _:
                    if(_token != null){
                        if(_token.ValidTo < DateTime.Now){
                            Sender.Tell(_token, Self);
                            break;
                        }
                        _token = null;
                        _http.Tell(_auth.RefreshTokenMessage(_refresh_token),Self);
                    }
                    _waitingForToken.Add(Sender);
                    break;
                case Response res:
                    var json = res.JsonBody();
                    _token = new JwtSecurityToken((string)json["access_token"]);
                    _refresh_token = (string)json["refresh_token"];
                    foreach (var sender in _waitingForToken)
                    {
                        sender.Tell(_token, Self);
                    }
                    _waitingForToken.Clear();
                    break;
            }
        }

        public static Props Props(IActorRef http, OAuthMs auth){
            return Akka.Actor.Props.Create(() => new OAuthMsActor(http, auth));
        }
    }
}