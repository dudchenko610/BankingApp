using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
using BankingApp.UI.Core.Wrappers;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingApp.UI
{
    /// <summary>
    /// Contains program configuration.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var apiBaseAddress = builder.Configuration["ApiBaseAddress"];
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

            ConfigureServices(builder.Services);

            var host = builder.Build();
            var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
            await authenticationService.InitializeAsync();

            await host.RunAsync();
        }

        /// <summary>
        /// Configures DI container.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoaderService, LoaderService>();
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<INavigationWrapper, NavigationWrapper>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IUserService, UserService>();

            services.AddBlazoredToast();
            services.AddBlazoredLocalStorage();
        }
    }
}
