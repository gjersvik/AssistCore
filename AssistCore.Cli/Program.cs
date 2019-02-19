using System;
using Akka.Actor;
using AssistCore.Http;

namespace AssistCore.Cli
{
    public class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("ActorCoreTestCli");
            var http = system.ActorOf<ManagerActor>();
            http.Tell(new Bind(system.ActorOf<HelloWorldActor>(), "http://localhost:8080/"));
            Console.ReadLine();
        }
    }

    public class HelloWorldActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if(message is Request req){
                Sender.Tell(Response.Text("Hello World!"));
            }
        }
    }
}
