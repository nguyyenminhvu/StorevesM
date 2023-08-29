using StorevesM.ProductService.Model.DTOMessage;

namespace StorevesM.ProductService.Grpc.Service.Interface
{
    public interface ICategoryServiceSupport
    {
        Task<CategoryDTO> GetCategory(int id);
    }
}
