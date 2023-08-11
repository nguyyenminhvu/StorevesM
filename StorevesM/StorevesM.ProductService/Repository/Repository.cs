using Microsoft.EntityFrameworkCore;
using StorevesM.ProductService.Entity;
using System.Linq.Expressions;

namespace StorevesM.ProductService.Repository
{
    public class Repository<T> :IRepository<T> where T : class
    {

        private readonly ProductDbContext _context;
        private readonly DbSet<T> _entity;

        public Repository(ProductDbContext appDbContext)
        {
            _context = appDbContext;
            _entity = _context.Set<T>();
        }
        public async Task AddAsync(T item)
        {
            await _entity.AddAsync(item);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> include = null)
        {
            return include == null! ? await _entity.FirstOrDefaultAsync(predicate) : await _entity.Include(include).FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _entity;
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> include = null!)
        {
            return include == null! ? _entity.Where(predicate) : _entity.Where(predicate).Include(include);
        }

        public void Remove(T entity)
        {
            _entity.Remove(entity);
        }

        public void RemoveRangeAsync(IEnumerable<T> entities)
        {
            _entity.RemoveRange(entities);
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }


    }
}
