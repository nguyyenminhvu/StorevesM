using StorevesM.ProductService.Model.DTOMessage;
using StorevesM.ProductService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        Task<CategoryDTO> GetCategory(MessageRaw raw, CancellationToken cancellation = default);
        Task ResponseGetProducts(MessageRaw raw, CancellationToken cancellation = default);
        Task ResponseUpdateQuantity(MessageRaw raw, CancellationToken cancellationToken = default);
    }
}
