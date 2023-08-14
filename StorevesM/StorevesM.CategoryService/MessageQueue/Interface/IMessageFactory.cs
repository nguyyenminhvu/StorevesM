namespace StorevesM.CategoryService.MessageQueue.Interface
{
    public interface IMessageFactory
    {
        Task ProcessMessage(string messageRaw);
    }
}
