using StorevesM.CategoryService.Entity;
using StorevesM.CategoryService.Repository.Implement;
using StorevesM.CategoryService.Repository.Interface;

namespace StorevesM.CategoryService.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CategoryDbContext _context;

        public ICategoryRepository CategoryRepository { get; private set; }
        public UnitOfWork(CategoryDbContext categoryDb)
        {
            _context = categoryDb;
            CategoryRepository = new CategoryRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
