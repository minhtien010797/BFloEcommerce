

using BFloEcommerceClient.Protos;
using Grpc.Net.Client;

await AssemblyLoadEventHandler();

static async Task AssemblyLoadEventHandler ()
{
    using var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client = new TodoService.TodoServiceClient(channel);

    var reply = await client.TodoSendAsync(
        new TodoRequest { Name = "Hello SWN Client"}
        );


    Console.WriteLine("Logging: "+ reply.Message);
    Console.ReadKey();
}