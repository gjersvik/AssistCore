using System.Net.Http.Headers;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.Graph;

namespace AssistCore.Services.MsGraph
{
    public class Client: UntypedActor
    {
        private GraphServiceClient _client;
        private string _accessToken = "";

        public Client(){
            _client = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) => {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("bearer", _accessToken);

                return Task.FromResult(0);
            }));
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case UpdateAccessToken update:
                    _accessToken = update.Token;
                    break;
                default:
                    Unhandled(message);
                    break;
            }
        }
    }
}