using Microsoft.EntityFrameworkCore;
using StorevesM.CustomerService.Entity;

namespace StorevesM.CustomerService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomerDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("CustomerDb")));
        }
    }
}
