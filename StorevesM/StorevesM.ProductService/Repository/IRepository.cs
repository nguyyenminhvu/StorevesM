using System.Linq.Expressions;

namespace StorevesM.ProductService.Repository
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T item);

        IQueryable<T> GetAll();

        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> include = null!);

        Task<int> SaveChangeAsync();

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> include = null!);

        void Remove(T entity);

        void RemoveRangeAsync(IEnumerable<T> entities);


    }
}
