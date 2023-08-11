using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StorevesM.ProductService.Entity;
using StorevesM.ProductService.Model.Request;
using StorevesM.ProductService.Model.View;
using StorevesM.ProductService.Repository;
using System.Linq;

namespace StorevesM.ProductService.Service
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepository;

        public ProductService(ProductDbContext productDb, IMapper mapper)
        {
            _context = productDb;
            _mapper = mapper;
            _productRepository = new Repository<Product>(_context);
        }

        public async Task<ProductViewModel> CreateProduct(ProductCreateModel productCreate)
        {
            Product product = new Product();
            product.Name = productCreate.Name;
            product.Describe = productCreate.Describe;
            product.Price = productCreate.Price;
            product.Quantity = productCreate.Quantity;
            product.CategoryId = productCreate.CategoryId;
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangeAsync();
            return await GetProduct(product.Id);
        }

        public async Task<ProductViewModel> GetProduct(int id)
        {
            var product = await _productRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            return product != null ? _mapper.Map<ProductViewModel>(product) : null!;
        }

        public async Task<List<ProductViewModel>> GetProducts(ProductFilterModel productFilter)
        {
            var queryable = _productRepository.GetAll().Where(x => x.IsActive);
            if (queryable.Count() <= 0)
            {
                return null!;
            }
            if (productFilter.Name != null && productFilter.Name != string.Empty)
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(productFilter.Name.ToLower()));
            }
            if (productFilter.PriceFrom.HasValue)
            {
                queryable = queryable.Where(x => x.Price >= productFilter.PriceFrom);
            }
            if (productFilter.PriceTo.HasValue)
            {
                queryable = queryable.Where(x => x.Price <= productFilter.PriceTo);
            }

            // Missed filter by categoryId

            if (queryable.Count() <= 0)
            {
                return null!;
            }
            return await queryable.ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> RemoveProduct(int id)
        {
            var product = await _productRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null!)
            {
                return false;
            }
            product.IsActive = false;
            await _productRepository.SaveChangeAsync();
            return true;
        }

        public async Task<ProductViewModel> UpdateProduct(ProductUpdateModel productUpdate, int id)
        {
            var product = await _productRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null!)
            {
                return null!;
            }
            product.Name = productUpdate.Name ?? product.Name;
            product.Describe = productUpdate.Describe ?? product.Describe;
            product.Price = productUpdate.Price ?? product.Price;
            product.CategoryId = productUpdate.CategoryId ?? product.CategoryId;
            await _productRepository.SaveChangeAsync();
            return await GetProduct(id);
        }
    }
}
