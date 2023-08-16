using StorevesM.OrderService.MessageQueue.Implement;
using StorevesM.OrderService.MessageQueue.Interface;

namespace StorevesM.OrderService.ApplicationConfig
{
    public static class SubcribeMessage
    {
        public static void SubcribeMessageQueue(this IServiceCollection services, IConfiguration configuration, IMessageFactory messageFactory)
        {

        }
    }
}
