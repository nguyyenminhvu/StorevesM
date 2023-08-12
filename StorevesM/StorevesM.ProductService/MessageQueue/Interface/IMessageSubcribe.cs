using StorevesM.ProductService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageSubcribe
    {
        Task<string> SubcribeMessageBroker(MessageChanel messageChanel, CancellationToken stoppingToken);
    }
}
