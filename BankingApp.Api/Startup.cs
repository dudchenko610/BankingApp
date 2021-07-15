using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BankingApp.Api.Validators.Banking;
using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.ViewModels.Banking.Account;

namespace BankingApp.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();
            BankingApp.BusinessLogicLayer.Startup.Initialize(services, Configuration);

            services.AddTransient<IValidator<CalculateDepositView>, CalculateDepositeViewValidator>();
            services.AddTransient<IValidator<SignInAccountView>, SignInAccountViewValidator>();
            services.AddTransient<IValidator<SignUpAccountView>, SignUpAccountViewValidator>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                builder.WithOrigins("https://localhost:44346")
                       .AllowAnyMethod()
                       .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
