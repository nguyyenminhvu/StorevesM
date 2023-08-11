using Microsoft.EntityFrameworkCore;
using StorevesM.ProductService.Entity;

namespace StorevesM.ProductService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("ProductDb")));
        }
    }
}
