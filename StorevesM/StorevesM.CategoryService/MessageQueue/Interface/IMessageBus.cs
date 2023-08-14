using StorevesM.CategoryService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageBus
    {
        void PublicMessage(MessageRaw raw);
    }
}
