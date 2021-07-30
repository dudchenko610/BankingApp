using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BankingApp.Api.Validators;
using BankingApp.ViewModels.ViewModels.Authentication;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.Banking.Admin;

namespace BankingApp.Api
{
    /// <summary>
    /// Contains program configuration.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gives access to appsetting files.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Creates instance of <see cref="Startup"/> class.
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures DI container.
        /// </summary>
        /// <param name="services">Used to connect services with dependency injection container.</param>
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
            services.AddTransient<IValidator<BlockUserAdminView>, BlockUserAdminViewValidator>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                builder.WithOrigins("https://localhost:44346")
                       .AllowAnyMethod()
                       .AllowAnyHeader());
            });
        }

        /// <summary>
        /// Configures middlewares.
        /// </summary>
        /// <param name="app">Used to connect middlewares to application's request pipeline.</param>
        /// <param name="env">Used to get information about the web hosting environment.</param>
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
