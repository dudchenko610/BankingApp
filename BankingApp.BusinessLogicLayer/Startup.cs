using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.BusinessLogicLayer.Mapper;
using BankingApp.BusinessLogicLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.BusinessLogicLayer
{
    public class Startup
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            DataAccessLayer.Startup.Initialize(services, configuration);

            services.AddTransient<IDepositService, DepositService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}

