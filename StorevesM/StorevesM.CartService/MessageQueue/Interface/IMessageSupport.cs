using StorevesM.CartService.Model.DTOMessage;
using StorevesM.CartService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        // Task<bool> CheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default);
        Task<CategoryDTO> GetCategory(MessageRaw raw, CancellationToken cancellation = default);
    }
}
