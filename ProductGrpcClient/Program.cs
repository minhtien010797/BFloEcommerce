using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpc.Protos;

await GetProductByIdAsync();

static async Task GetProductByIdAsync()
{
    Console.WriteLine("Waiting for server is running....");
    Thread.Sleep(2000);

    using var channel = GrpcChannel.ForAddress("http://localhost:5002");
    var client = new ProductProtoService.ProductProtoServiceClient(channel);

    await GetProductAsync(client);
    await GetAllProductsAsync(client);

    Console.ReadLine();
}

static async Task GetAllProductsAsync(ProductProtoService.ProductProtoServiceClient client)
{
    //// GetAllProducts
    //Console.WriteLine("GetAllProducts started..........");
    //using (var clientData = client.GetAllProducts(new GetAllProductsRequest()))
    //{
    //    while (await clientData.ResponseStream.MoveNext(new CancellationToken()))
    //    {
    //        var currentProduct = clientData.ResponseStream.Current;
    //        Console.WriteLine(currentProduct);
    //    }
    //}

    // GetAllProducts C#9
    Console.WriteLine("GetAllProducts started..........");
    using var clientData = client.GetAllProducts(new GetAllProductsRequest());
    {
        await foreach (var responseData in clientData.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine(responseData);
        }
    };
}

static async Task GetProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    // GetProductById
    Console.WriteLine("GetProductById started..........");
    var reply = await client.GetProductByIdAsync(
        new GetProductRequest
        {
            ProductId = Guid.Parse("dc7f6668-5b47-40b7-a94a-b2332cbf8186").ToString()
        });


    Console.WriteLine("GetProductById replied: " + reply.ToString());
}