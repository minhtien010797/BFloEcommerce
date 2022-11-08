

using Grpc.Net.Client;
using GrpcGreeterClient;

await AssemblyLoadEventHandler();

static async Task AssemblyLoadEventHandler()
{
    using var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client = new Greeter.GreeterClient(channel);

    var reply = await client.SayHelloAsync(
        new HelloRequest { Name = "Hello SWN Client" }
        );


    Console.WriteLine("Greetings: " + reply.Message);
    Console.ReadKey();
}