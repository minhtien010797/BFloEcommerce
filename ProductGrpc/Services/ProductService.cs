using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductGrpc.Data;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ProductsContext _productsContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductsContext productsContext, ILogger<ProductService> logger)
        {
            _productsContext = productsContext;
            _logger = logger;
        }

        public override Task<ProductModel> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            return base.AddProduct(request, context);
        }

        public override Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            return base.DeleteProduct(request, context);
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override Task GetAllProducts(GetAllProductsRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            return base.GetAllProducts(request, responseStream, context);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override async Task<ProductModel> GetProductById(GetProductRequest request, ServerCallContext context)
        {
            var product = await _productsContext.Product.FindAsync(request.ProductId);

            ArgumentNullException.ThrowIfNull(request.ProductId);

            return new ProductModel
            {
                ProductId = product.ProductId.ToString(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Status = ProductStatus.Instock,
                CreatedTime = Timestamp.FromDateTime(product.CreatedTime)
            };
        }

        public override Task<InsertBulkProductResponse> InsertBulkProduct(IAsyncStreamReader<ProductModel> requestStream, ServerCallContext context)
        {
            return base.InsertBulkProduct(requestStream, context);
        }

        public override Task<Empty> Test(Empty request, ServerCallContext context)
        {
            return base.Test(request, context);
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override Task<ProductModel> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            return base.UpdateProduct(request, context);
        }
    }
}
