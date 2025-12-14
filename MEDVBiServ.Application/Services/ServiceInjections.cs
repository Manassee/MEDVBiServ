using MEDVBiServ.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEDVBiServ.Application.Services;

namespace MEDVBiServ.Application.Services
{
    public static class ServiceInjections
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBibleService, BibleService>();
            services.AddScoped<ILanguageProvider, LanguageProvider>();

            return services;
        }
    }
}
