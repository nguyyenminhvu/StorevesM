using Microsoft.EntityFrameworkCore;
using StorevesM.OrderService.Entity;
using StorevesM.OrderService.MessageQueue.Implement;
using StorevesM.OrderService.MessageQueue.Interface;
using StorevesM.OrderService.Profiles;
using StorevesM.OrderService.Service;

namespace StorevesM.OrderService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("OrderDb")));
            services.AddScoped<IOrderService, OrderService.Service.OrderService>();
            services.AddAutoMapper(typeof(ProfileMapper));
            services.AddSingleton<IMessageSupport, MessageSupport>();
            services.AddSingleton<IMessageFactory, MessageFactory>();
        }
    }
}
