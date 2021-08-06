using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Repositories;
using BankingApp.DataAccessLayer.Services;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.DataAccessLayer
{
    /// <summary>
    /// Contains configuration for data access layer services.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures DI container for data access layer services.
        /// </summary>
        /// <param name="services">Used to connect services with dependency injection container.</param>
        /// <param name="configuration">Gives access to appsetting files.</param>
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AdminCredentialsOptions>(configuration.GetSection(Constants.AppSettings.AdminCredentials));
            string connection = configuration.GetConnectionString(Constants.AppSettings.SqlServerConnection);
            services.AddDbContext<BankingDbContext>(builder =>
                builder.UseSqlServer(connection, x => x.MigrationsAssembly(Constants.Assembly.DataAccessLayer))
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
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDataSeederService, DataSeederService>();

            using (var context = services.BuildServiceProvider().GetService<BankingDbContext>())
            { 
                context.Database.Migrate();
            }

            var dataSeederService = services.BuildServiceProvider().GetService<IDataSeederService>();
            dataSeederService.SeedDataAsync().Wait();
        }
    }
}
