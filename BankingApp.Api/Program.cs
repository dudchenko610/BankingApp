using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BankingApp.Api
{
    /// <summary>
    /// Contains program entry point - method Main.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program entry point. Runs server host and blocks until host shutdown.
        /// </summary>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Configures host using as configuration <see cref="Startup"/> class.
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        } 
    }
}
