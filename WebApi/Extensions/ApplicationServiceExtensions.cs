using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,IConfiguration config){

            services.AddDbContext<ApplicationDataContext>(option => {
                 option.UseNpgsql(config.GetConnectionString("DefaultString"));              
            });

            return services;
        }
    }
}