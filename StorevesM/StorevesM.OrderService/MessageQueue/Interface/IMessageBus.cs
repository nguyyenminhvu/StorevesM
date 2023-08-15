using StorevesM.OrderService.Model.Message;

namespace StorevesM.OrderService.MessageQueue.Interface
{
    public interface IMessageBus
    {
        void PublicMessage(MessageRaw raw);
    }
}
