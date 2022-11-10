using ProductGrpc.Models;

namespace ProductGrpc.Data
{
    public class ProductsContextSeed
    {
        public static void SeedAsync(ProductsContext productsContext)
        {
            if (!productsContext.Product.Any())
            {
                var products = new List<Product>
                {
                    new Product{
                        ProductId = Guid.Parse("dc7f6668-5b47-40b7-a94a-b2332cbf8186"),
                        Name = "Product 01",
                        Description = "New Product 01",
                        Price = 799,
                        Status = ProductStatus.INSTOCK,
                        CreatedTime = DateTime.UtcNow
                    },
                    new Product{
                        ProductId = Guid.Parse("bfc26f31-150c-4eac-8a80-19d8203691d1"),
                        Name = "Product 02",
                        Description = "New Product 02",
                        Price = 799,
                        Status = ProductStatus.INSTOCK,
                        CreatedTime = DateTime.UtcNow
                    },
                    new Product{
                        ProductId = Guid.Parse("b13ae9ea-d436-4e3d-8e38-40ca2064823b"),
                        Name = "Product 03",
                        Description = "New Product 03",
                        Price = 799,
                        Status = ProductStatus.INSTOCK,
                        CreatedTime = DateTime.UtcNow
                    }
                };

                productsContext.Product.AddRange(products);
                productsContext.SaveChanges();
            }
        }
    }
}
