using Google.Protobuf.WellKnownTypes;
using ProductGrpc.Protos;

namespace ProductWorkerService
{
    public class ProductFactory
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public ProductFactory(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task<AddProductRequest> GenerateProduct()
        {
            var productName = $"{_configuration.GetValue<string>("WorkerService:ProductName")}_{DateTimeOffset.UtcNow}";
            return Task.FromResult(new AddProductRequest
            {
                Product = new ProductModel
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Name = productName,
                    Description = $"{productName}_Description",
                    Price = new Random().Next(1000),
                    Status = ProductStatus.Instock,
                    CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
                }
            });
        }
    }
}
