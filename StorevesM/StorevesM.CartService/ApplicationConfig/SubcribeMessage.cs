using StorevesM.CartService.MessageQueue.Implement;
using StorevesM.CartService.MessageQueue.Interface;
using StorevesM.CartService.Model.Message;

namespace StorevesM.CartService.ApplicationConfig
{
    public static class SubcribeMessage
    {
        public static void SubcribeMessageQueue(this IServiceCollection services, IConfiguration configuration, IMessageFactory messageFactory)
        {
            services.AddHostedService<MessageSubcribe>();
            MessageChanel messageChanel = new();

            services.AddSingleton<MessageChanel>(messageChanel.ClearCartItem());
            

        }
    }
}
