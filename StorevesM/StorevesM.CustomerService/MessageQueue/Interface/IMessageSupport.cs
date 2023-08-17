using StorevesM.CustomerService.Model.Message;

namespace StorevesM.CustomerService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        Task ResponseGetCustomer(MessageRaw raw, CancellationToken cancellation = default);
    }
}
