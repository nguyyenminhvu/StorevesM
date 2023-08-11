using StorevesM.ProductService.Model.Request;
using StorevesM.ProductService.Model.View;

namespace StorevesM.ProductService.Service
{
    public interface IProductService
    {
        Task<ProductViewModel> GetProduct(int id);
        Task<List<ProductViewModel>> GetProducts(ProductFilterModel productFilter);
        Task<ProductViewModel> CreateProduct(ProductCreateModel productCreate);
        Task<ProductViewModel> UpdateProduct(ProductUpdateModel productUpdate, int id);
        Task<bool> RemoveProduct(int id);
    }
}
