using Grpc.Net.Client;
using ProductGrpc.Protos;

await GetProductByIdAsync();

static async Task GetProductByIdAsync()
{
    Console.WriteLine("Waiting for server is running....");
    Thread.Sleep(2000);

    using var channel = GrpcChannel.ForAddress("http://localhost:5002");
    var client = new ProductProtoService.ProductProtoServiceClient(channel);


    Console.WriteLine("GetProductById started..........");
    var reply = await client.GetProductByIdAsync(
        new GetProductRequest
        {
            ProductId = Guid.Parse("dc7f6668-5b47-40b7-a94a-b2332cbf8186").ToString()
        });


    Console.WriteLine("GetProductById replied: " + reply.ToString());
    Console.ReadLine();
}