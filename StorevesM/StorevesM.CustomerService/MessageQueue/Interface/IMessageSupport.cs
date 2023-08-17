using StorevesM.CustomerService.Model.DTOMessage;
using StorevesM.CustomerService.Model.Message;

namespace StorevesM.CustomerService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        // Task<bool> CheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default);
        Task<CategoryDTO> GetCategory(MessageRaw raw, CancellationToken cancellation = default);
        Task<bool> UpdateQuantityProduct(MessageRaw raw, CancellationToken cancellation = default);
        Task<List<ProductDTO>> GetProducts(MessageRaw raw, CancellationToken cancellationToken = default);
        Task<bool> ClearCartItem(MessageRaw raw, CancellationToken cancellation = default);
    }
}
