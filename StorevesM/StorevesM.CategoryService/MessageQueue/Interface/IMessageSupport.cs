using StorevesM.CategoryService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        Task ResponseCheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default);
    }
}
