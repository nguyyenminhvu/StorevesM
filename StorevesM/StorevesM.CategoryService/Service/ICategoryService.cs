using StorevesM.CategoryService.Model.Request;
using StorevesM.CategoryService.Model.View;

namespace StorevesM.CategoryService.Service
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> GetCategory(int id);
        Task<List<CategoryViewModel>> GetCategories(CategoryFilter categoryFilter);
        Task<CategoryViewModel> UpdateCategory(CategoryUpdateModel categoryUpdate);
        Task<CategoryViewModel> CreateCategory(CategoryCreateModel categoryCreateModel);
    }
}
