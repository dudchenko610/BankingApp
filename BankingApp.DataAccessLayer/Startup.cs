using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.DataAccessLayer
{
    public class Startup
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("SQLServerConnection");
            services.AddDbContext<BankingDbContext>(builder =>
                builder.UseSqlServer(connection, x => x.MigrationsAssembly("BankingApp.DataAccessLayer"))
            );

            services.AddTransient<IDepositRepository, DepositRepository>();
        }
    }
}
