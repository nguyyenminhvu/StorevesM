using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StorevesM.CategoryService.Entity;
using StorevesM.CategoryService.Model.Request;
using StorevesM.CategoryService.Model.View;
using StorevesM.CategoryService.UnitOfWork;

namespace StorevesM.CategoryService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<CategoryViewModel> CreateCategory(CategoryCreateModel categoryCreateModel)
        {
            Category category = new Category();
            category.Name = categoryCreateModel.Name!;
            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return await GetCategory(category.Id);
        }

        public async Task<List<CategoryViewModel>> GetCategories(CategoryFilter categoryFilter)
        {
            var queryable = _unitOfWork.CategoryRepository.GetAll();
            if (queryable.Count() > 0)
            {
                if (categoryFilter.Name != null)
                {
                    queryable = queryable.Where(x => x.Name.ToLower().Contains(categoryFilter.Name.ToLower()));
                }
            }
            if (queryable.Count() <= 0)
            {
                return null!;
            }
            return await queryable.ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<CategoryViewModel> GetCategory(int id)
        {
            var category = await _unitOfWork.CategoryRepository.FirstOrDefaultAsync(x => x.Id == id);
            return category != null! ? _mapper.Map<CategoryViewModel>(category) : null!;
        }

        public async Task<CategoryViewModel> UpdateCategory(CategoryUpdateModel categoryUpdate)
        {
            var category = await _unitOfWork.CategoryRepository.FirstOrDefaultAsync(x => x.Id == categoryUpdate.Id);
            if (category == null!)
            {
                return null!;
            }
            category.Name = categoryUpdate.Name ?? category.Name;
            await _unitOfWork.SaveChangesAsync();
            return await GetCategory(categoryUpdate.Id);
        }
    }
}
