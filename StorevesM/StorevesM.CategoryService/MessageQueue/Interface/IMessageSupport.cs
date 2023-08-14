using StorevesM.CategoryService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        Task<bool> CheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default);
        Task ResponseCheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default);
    }
}
