using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.BusinessLogicLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp.BusinessLogicLayer
{
    public class Startup
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            BankingApp.DataAccessLayer.Startup.Initialize(services, configuration);

            services.AddTransient<IBankingService, BankingService>();
        }
    }
}

