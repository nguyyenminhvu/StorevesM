namespace StorevesM.OrderService.MessageQueue.Interface
{
    public interface IMessageFactory
    {
        Task ProcessMessage(string messageRaw);
    }
}
