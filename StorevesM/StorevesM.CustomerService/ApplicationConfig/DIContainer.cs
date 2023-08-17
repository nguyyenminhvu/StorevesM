using Microsoft.EntityFrameworkCore;
using StorevesM.CustomerService.Entity;
using StorevesM.CustomerService.MessageQueue.Implement;
using StorevesM.CustomerService.MessageQueue.Interface;
using StorevesM.CustomerService.Model.Message;
using StorevesM.CustomerService.Profiles;
using StorevesM.CustomerService.Service;

namespace StorevesM.CustomerService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomerDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("CustomerDb")));
            services.AddAutoMapper(typeof(ProfileMapper));
            services.AddScoped<ICustomerService, CustomerService.Service.CustomerService>();

            services.AddSingleton<IMessageFactory, MessageFactory>();
            services.AddSingleton<IMessageSupport, MessageSupport>();
            services.AddSingleton<MessageChanel>();

          
        }
    }
}
