using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Repositories;
using BankingApp.Entities.Entities;
using Microsoft.AspNetCore.Identity;
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

            services.AddIdentity<User, IdentityRole<int>>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<BankingDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            services.AddScoped<RoleManager<IdentityRole<int>>>();

            services.AddTransient<IDepositRepository, DepositRepository>();
        }
    }
}
