using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using ProductGrpc.Protos;

namespace ProductWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Waiting for server is running....");
            Thread.Sleep(2000);

            while (!stoppingToken.IsCancellationRequested)
            {
                var intervalTask = _configuration.GetValue<int>("WorkerService:TaskInterval");

                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new ProductProtoService.ProductProtoServiceClient(channel);

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(intervalTask, stoppingToken);
            }
        }
    }
}