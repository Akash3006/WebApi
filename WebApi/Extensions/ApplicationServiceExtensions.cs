using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,IConfiguration config){

            services.AddScoped<ITokenService,TokenService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IPhotoService,PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddDbContext<ApplicationDataContext>(option => {
                 option.UseNpgsql(config.GetConnectionString("DefaultString"));              
            });
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            return services;
        }
    }
}