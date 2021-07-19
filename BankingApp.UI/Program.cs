using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
using BankingApp.UI.Core.Wrappers;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingApp.UI
{
    public class Program
    {
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

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoaderService, LoaderService>();
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<INavigationWrapper, NavigationWrapper>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILocalStorageService, LocalStorageService>();
            services.AddScoped<IHttpService, HttpService>();

            services.AddBlazoredToast();
        }
    }
}
