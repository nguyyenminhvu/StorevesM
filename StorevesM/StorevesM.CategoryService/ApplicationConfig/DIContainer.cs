using Microsoft.EntityFrameworkCore;
using StorevesM.CategoryService.Entity;
using StorevesM.CategoryService.Profiles;
using StorevesM.CategoryService.Service;
using StorevesM.CategoryService.UnitOfWork;

namespace StorevesM.CategoryService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectionDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CategoryDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("CategoryDb")));
            services.AddAutoMapper(typeof(ProfileMapper));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<ICategoryService, CategoryService.Service.CategoryService>();
        }
    }
}
