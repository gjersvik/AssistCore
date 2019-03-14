namespace AssistCore.Services.MsGraph
{
    public sealed class UpdateAccessToken{
        public readonly string Token;

        public UpdateAccessToken(string token){
            Token = token;
        }
    }
}