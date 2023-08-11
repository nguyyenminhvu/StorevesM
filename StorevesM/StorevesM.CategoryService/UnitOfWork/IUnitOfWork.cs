using StorevesM.CategoryService.Repository.Interface;

namespace StorevesM.CategoryService.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
