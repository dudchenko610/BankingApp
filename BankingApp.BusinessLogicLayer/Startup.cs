using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApp.BusinessLogicLayer
{
    public class Startup
    {

        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            BankingApp.DataAccessLayer.Startup.Initialize(services, configuration);
        }
    }
}
