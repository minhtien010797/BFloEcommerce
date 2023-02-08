using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data;
using ProductGrpc.Models;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ProductsContext _productsContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(ProductsContext productsContext, ILogger<ProductService> logger, IMapper mapper)
        {
            _productsContext = productsContext ?? throw new ArgumentNullException(nameof(productsContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }

        public override async Task<ProductModel> AddOrUpdateProduct(AddOrUpdateProductRequest request, ServerCallContext context)
        {
            var product = await _productsContext.Product
                .Where(p => p.ProductId == Guid.Parse(request.Product.ProductId))
                .FirstOrDefaultAsync();

            if (product != null)
            {
                return await UpdateProduct(new UpdateProductRequest { Product = request.Product }, context);
            }
            else
            {
                return await AddProduct(new AddProductRequest { Product = request.Product }, context);
            }
        }

        public override async Task<ProductModel> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            // using AutoMapper to map model
            var product = _mapper.Map<Product>(request.Product);

            // add product and update with SaveChange()
            _productsContext.Product.Add(product);
            await _productsContext.SaveChangesAsync();

            // using AutoMapper to map model
            var productModel = _mapper.Map<ProductModel>(product);
            return productModel;
        }

        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            var product = await _productsContext.Product.FindAsync(Guid.Parse(request.ProductId));
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"The Enity doesn't exist in system with Id: {request.ProductId}"));
            }

            _productsContext.Product.Remove(product);
            var deleteCount = await _productsContext.SaveChangesAsync();

            var response = new DeleteProductResponse
            {
                Success = deleteCount > 0
            };

            return response;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override async Task GetAllProducts(GetAllProductsRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            var productList = await _productsContext.Product.ToListAsync();

            foreach (var item in productList)
            {
                // using AutoMapper to map model
                var product = _mapper.Map<ProductModel>(item);

                await   responseStream.WriteAsync(product);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override async Task<ProductModel> GetProductById(GetProductRequest request, ServerCallContext context)
        {
            var product = await _productsContext.Product.FindAsync(Guid.Parse(request.ProductId));

            // Throw exception if enity's null.
            ArgumentNullException.ThrowIfNull(request.ProductId);

            return new ProductModel
            {
                ProductId = product.ProductId.ToString(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Status = Protos.ProductStatus.Instock,
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

        public override async Task<ProductModel> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            // Mapping model without AutoMapper.
            var product = new Product
            {
                ProductId = Guid.Parse(request.Product.ProductId),
                Name = request.Product.Name,
                Description = request.Product.Description,
                Price = request.Product.Price,
                Status = Models.ProductStatus.INSTOCK,
                CreatedTime = request.Product.CreatedTime.ToDateTime()
            };

            var isExist = await _productsContext.Product.AnyAsync(p => p.ProductId == product.ProductId);
            if (!isExist)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"The Enity doesn't exist in system with Id: {product.ProductId}"));
            }

            _productsContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _productsContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            var productModel = new ProductModel
            {
                ProductId = product.ProductId.ToString(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Status = Protos.ProductStatus.Instock,
                CreatedTime = Timestamp.FromDateTime(product.CreatedTime)
            };

            return productModel;
        }
    }
}
