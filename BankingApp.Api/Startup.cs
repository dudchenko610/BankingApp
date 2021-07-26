using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.ViewModels.Banking.Authentication;
using BankingApp.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using BankingApp.Api.Validators;

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
            BankingApp.BusinessLogicLayer.Startup.Initialize(services, Configuration);

            services.AddControllers().AddFluentValidation();

            services.AddTransient<IValidator<CalculateDepositView>, CalculateDepositeViewValidator>();
            services.AddTransient<IValidator<SignInAuthenticationView>, SignInAuthenticationViewValidator>();
            services.AddTransient<IValidator<SignUpAuthenticationView>, SignUpAuthenticationViewValidator>();
            services.AddTransient<IValidator<ResetPasswordAuthenticationView>, ResetPasswordAuthenticationViewValidator>();
            services.AddTransient<IValidator<ForgotPasswordAuthenticationView>, ForgotPasswordAuthenticationViewValidator>();
            services.AddTransient<IValidator<ConfirmEmailAuthenticationView>, ConfirmEmailAuthenticationViewValidator>();

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

            app.UseAuthentication();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
