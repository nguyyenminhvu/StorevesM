using StorevesM.OrderService.Model.DTOMessage;
using StorevesM.OrderService.Model.Message;

namespace StorevesM.OrderService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        // Task<bool> CheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default);
        Task<CategoryDTO> GetCategory(MessageRaw raw, CancellationToken cancellation = default);
    }
}
