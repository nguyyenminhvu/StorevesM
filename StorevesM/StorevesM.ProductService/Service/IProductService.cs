using StorevesM.ProductService.Model.DTOMessage;
using StorevesM.ProductService.Model.Request;
using StorevesM.ProductService.Model.View;

namespace StorevesM.ProductService.Service
{
    public interface IProductService
    {
        Task<ProductViewModel> GetProduct(int id);
        Task<List<ProductViewModel>> GetProducts(ProductFilterModel productFilter=null!);
        Task<ProductViewModel> CreateProduct(ProductCreateModel productCreate);
        Task<ProductViewModel> UpdateProduct(ProductUpdateModel productUpdate, int id);
        Task<bool> RemoveProduct(int id);
        Task<bool> SendMessageDemo();
        Task<bool> UpdateQuantityProduct(CartDTO cart);
    }
}
