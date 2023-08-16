using Microsoft.EntityFrameworkCore;
using StorevesM.CategoryService.MessageQueue.Implement;
using StorevesM.CategoryService.MessageQueue.Interface;
using StorevesM.ProductService.Entity;
using StorevesM.ProductService.MessageQueue.Implement;
using StorevesM.ProductService.MessageQueue.Interface;
using StorevesM.ProductService.Model.Message;
using StorevesM.ProductService.Profiles;
using StorevesM.ProductService.Service;

namespace StorevesM.ProductService.ApplicationConfig
{
    public static class DIContainer
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("ProductDb")));
            services.AddScoped<IProductService, ProductService.Service.ProductService>();
            services.AddSingleton<IMessageSupport, MessageSupport>();
            services.AddSingleton<IMessageFactory, MessageFactory>();
            services.AddSingleton<MessageChanel>();
            services.AddAutoMapper(typeof(ProfileMapper));
        }
    }
}
