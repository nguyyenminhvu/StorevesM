using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StorevesM.CartService.Entity;
using StorevesM.CartService.MessageQueue.Implement;
using StorevesM.CartService.MessageQueue.Interface;
using StorevesM.CartService.Profiles;
using StorevesM.CartService.Service.Implement;
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
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<IMessageFactory, MessageFactory>();
            services.AddSingleton<IMessageSupport, MessageSupport>();

        }
        public static void RegisterJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["SecretKeyToken"]!))
                };
            });
        }
        public static void AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Microservices",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                    new OpenApiSecurityScheme{
                    Reference= new OpenApiReference{
                    Id="Bearer",
                    Type=ReferenceType.SecurityScheme
                    },Scheme="oauth2",
                    Name="Bearer",
                    In=ParameterLocation.Header,
                    },
                    new  List<string>()
                    }
                });
            });
        }
    }
}
