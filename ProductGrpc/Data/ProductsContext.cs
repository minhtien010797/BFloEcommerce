using Microsoft.EntityFrameworkCore;
using ProductGrpc.Models;

namespace ProductGrpc.Data
{
    public class ProductsContext : DbContext
    {
        protected ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}
