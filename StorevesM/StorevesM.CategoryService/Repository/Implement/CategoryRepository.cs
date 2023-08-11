using StorevesM.CategoryService.Entity;
using StorevesM.CategoryService.Repository.Interface;

namespace StorevesM.CategoryService.Repository.Implement
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CategoryDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
