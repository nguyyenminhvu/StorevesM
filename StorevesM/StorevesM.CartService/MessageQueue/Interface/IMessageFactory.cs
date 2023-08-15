namespace StorevesM.CartService.MessageQueue.Interface
{
    public interface IMessageFactory
    {
        Task ProcessMessage(string messageRaw);
    }
}
