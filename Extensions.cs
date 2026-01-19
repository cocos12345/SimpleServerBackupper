using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerBackupper
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder=> builder.AddConsole());
            services.Configure<Appsettings>(configuration);
            return services;
        }
    }
}
