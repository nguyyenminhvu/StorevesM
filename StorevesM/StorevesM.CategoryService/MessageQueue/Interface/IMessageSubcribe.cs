using StorevesM.CategoryService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageSubcribe
    {
        void SubcribeGetCategory(CancellationToken stoppingToken = default);
    }
}
