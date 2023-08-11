using StorevesM.ProductService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageBus
    {
        void PublicMessage(MessageRaw raw);
    }
}
