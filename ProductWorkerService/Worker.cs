using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using ProductGrpc.Protos;

namespace ProductWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly ProductFactory _productFactory;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, ProductFactory productFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _productFactory = productFactory ?? throw new ArgumentNullException(nameof(productFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Waiting for server is running....");
            Thread.Sleep(2000);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var intervalTask = _configuration.GetValue<int>("WorkerService:TaskInterval");
                var serverUrl = _configuration.GetValue<string>("WorkerService:ServerUrl");

                // Why do we have to add certificate here: https://github.com/grpc/grpc-dotnet/issues/792
                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var channel = GrpcChannel.ForAddress(serverUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
                var client = new ProductProtoService.ProductProtoServiceClient(channel);

                _logger.LogInformation("AddProductsAsync started....");
                var addProductResponse = await client.AddProductAsync(await _productFactory.GenerateProduct());
                _logger.LogInformation("AddProductsAsync Response: {0}", addProductResponse.ToString());

                await Task.Delay(intervalTask, stoppingToken);
            }
        }
    }
}