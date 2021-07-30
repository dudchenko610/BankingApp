using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.BusinessLogicLayer.Mapper;
using BankingApp.BusinessLogicLayer.Services;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankingApp.BusinessLogicLayer
{
    /// <summary>
    /// Contains configuration for bussiness layer services.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures DI container for bussiness layer services.
        /// </summary>
        /// <param name="services">Used to connect services with dependency injection container.</param>
        /// <param name="configuration">Gives access to appsetting files.</param>
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            DataAccessLayer.Startup.Initialize(services, configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IDepositService, DepositService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IJwtService, JwtService>();

            services.Configure<EmailConnectionOptions>(configuration.GetSection(Constants.AppSettings.EmailConfig));
            services.Configure<ClientConnectionOptions>(configuration.GetSection(Constants.AppSettings.ClientConfig));
            services.Configure<JwtOptions>(configuration.GetSection(Constants.AppSettings.JwtConfig));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
            .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(0),
                        ValidateIssuer = true,
                        ValidIssuer = configuration["jwtConfig:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["jwtConfig:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["jwtConfig:SecretKey"])),
                        ValidateIssuerSigningKey = true,
                    };
                });

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}

