using Microsoft.EntityFrameworkCore;

namespace StorevesM.CategoryService.Entity
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions<CategoryDbContext> opt) : base(opt)
        {

        }
        public DbSet<Category> Categories => Set<Category>();
    }
}
