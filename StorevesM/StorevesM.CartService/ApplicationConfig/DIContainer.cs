using Microsoft.EntityFrameworkCore;
using StorevesM.CartService.Entity;
using StorevesM.CartService.MessageQueue.Implement;
using StorevesM.CartService.MessageQueue.Interface;
using StorevesM.CartService.Profiles;
using StorevesM.CartService.Service.Interface;
using StorevesM.CategoryService.MessageQueue.Implement;
using StorevesM.ProductService.MessageQueue.Interface;

namespace StorevesM.CartService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CartServiceDbcontext>(x => x.UseSqlServer(configuration.GetConnectionString("CartDb")));
            services.AddAutoMapper(typeof(ProfileMapper));
            services.AddScoped<ICartService, CartService.Service.Implement.CartService>();

            services.AddSingleton<IMessageFactory, MessageFactory>();
            services.AddSingleton<IMessageSupport, MessageSupport>();

        }
    }
}
