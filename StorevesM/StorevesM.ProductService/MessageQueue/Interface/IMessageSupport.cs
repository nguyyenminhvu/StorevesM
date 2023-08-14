using StorevesM.ProductService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        Task<bool> CheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default);
    }
}
