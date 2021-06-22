using BusinessLogicLayer.Facade;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class Startup
    {

        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IBankingService, BankingService>();

            DataAccessLayer.Startup.Initialize(services, configuration);
        }
    }
}
