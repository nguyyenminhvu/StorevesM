using Microsoft.EntityFrameworkCore;

namespace StorevesM.ProductService.Entity
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> opt) : base(opt)
        {

        }
        public DbSet<Product> Products => Set<Product>();
    }
}
