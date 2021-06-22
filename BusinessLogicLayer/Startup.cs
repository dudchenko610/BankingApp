using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class Startup
    {

        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            DataAccessLayer.Startup.Initialize(services, configuration);
        }
    }
}
