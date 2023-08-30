using StorevesM.CategoryService.MessageQueue.Interface;
using StorevesM.ProductService.MessageQueue.Implement;
using StorevesM.ProductService.Model.Message;

namespace StorevesM.ProductService.ApplicationConfig
{
    public static class SubcribeMessage
    {
        public static void SubcribeMessageQueue(this IServiceCollection services, IConfiguration configuration, IMessageFactory messageFactory)
        {
            services.AddHostedService<MessageSubcribe>();
            MessageChanel messageChanel = new();
            services.AddSingleton<MessageChanel>(messageChanel.GetProducts());
            services.AddSingleton<MessageChanel>(messageChanel.UpdateQuantityProduct());

        }
    }
}
