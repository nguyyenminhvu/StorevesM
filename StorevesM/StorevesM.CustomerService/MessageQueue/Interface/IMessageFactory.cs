namespace StorevesM.CustomerService.MessageQueue.Interface
{
    public interface IMessageFactory
    {
        Task ProcessMessage(string messageRaw);
    }
}
