using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
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

           // var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var apiBaseAddress = builder.Configuration["ApiBaseAddress"];
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoaderService, LoaderService>();
            services.AddScoped<IDepositeService, DepositeService>();
        }
    }
}
