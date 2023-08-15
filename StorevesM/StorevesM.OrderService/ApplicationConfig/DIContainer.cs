using Microsoft.EntityFrameworkCore;
using StorevesM.OrderService.Entity;

namespace StorevesM.OrderService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("OrderDb")));
        }
    }
}
