using Microsoft.EntityFrameworkCore;
using StorevesM.CartService.Entity;
using StorevesM.CartService.Profiles;
using StorevesM.CartService.Service.Interface;

namespace StorevesM.CartService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CartServiceDbcontext>(x => x.UseSqlServer(configuration.GetConnectionString("CartDb")));
            services.AddAutoMapper(typeof(ProfileMapper));
            services.AddScoped<ICartService, CartService.Service.Implement.CartService>();
        }
    }
}
