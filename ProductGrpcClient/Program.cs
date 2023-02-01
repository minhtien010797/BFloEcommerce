using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpc.Protos;
// Run Main Method()
await RunConsole();

static async Task RunConsole()
{
    Console.WriteLine("Waiting for server is running....");
    Thread.Sleep(2000);

    using var channel = GrpcChannel.ForAddress("http://localhost:5002");
    var client = new ProductProtoService.ProductProtoServiceClient(channel);

    await GetProductAsync(client);
    await GetAllProductsAsync(client);
    await AddProductsAsync(client);

    Console.ReadLine();
}

static async Task AddProductsAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("AddProductsAsync started..........");
    var addProductResponse = await client.AddProductAsync
        (
            new AddProductRequest 
            {
                Product = new ProductModel
                {
                    ProductId = Guid.Parse("c2e32f6a-df89-42f6-97fa-48eb995c7524").ToString(),
                    Name = "Product 04",
                    Description = "New Product 04",
                    Price = 799,
                    Status = ProductStatus.Instock,
                    CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow) 
                }
            }
        );

    Console.WriteLine("AddProductsAsync Response: " + addProductResponse.ToString());
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