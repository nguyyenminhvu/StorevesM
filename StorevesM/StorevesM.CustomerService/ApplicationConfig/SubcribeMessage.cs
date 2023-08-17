using StorevesM.CustomerService.MessageQueue.Implement;
using StorevesM.CustomerService.Model.Message;

namespace StorevesM.CustomerService.ApplicationConfig
{
    public static class SubcribeMessage
    {
        public static void SubcribeMessageQueue(this IServiceCollection services)
        {
            services.AddHostedService<MessageSubcribe>();

            var messageChanel = new MessageChanel();
            services.AddSingleton<MessageChanel>(messageChanel.GetCustomer());
        }
    }
}
