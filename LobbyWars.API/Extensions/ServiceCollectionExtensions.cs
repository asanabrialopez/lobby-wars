using LobbyWars.Application.Services;
using LobbyWars.Domain.Repositories;
using LobbyWars.SharedKernel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LobbyWars.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JSON Web Token based security",
            };

            var securityReq = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            };

            var contact = new OpenApiContact()
            {
                Name = "Alexis Sanabria",
                Url = new Uri("https://www.linkedin.com/in/alexis-sanabria-lopez/"),
                Email = "asanabrial@outlook.com"
            };

            var info = new OpenApiInfo()
            {
                Version = "v1",
                Title = "Lobby Wars API",
                Description = "We are in the era of \"lawsuits\", everyone wants to go to court with their lawyer Saul and try to get a lot of dollars as if they were raining over\r\nManhattan.\r\nThe laws have changed much lately and governments have been digitized. That's when Signaturit comes into play.\r\nThe city council through the use of Signaturit maintains a registry of legal signatures of each party involved in the contracts that are made.\r\nDuring a trial, justice only verifies the signatures of the parties involved in the contract to decide who wins. For that, they assign points to the\r\ndifferent signatures depending on the role of who signed.",
                Contact = contact,
            };

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", info);
                o.AddSecurityDefinition("Bearer", securityScheme);
                o.AddSecurityRequirement(securityReq);
            });

            return services;
        }

        public static IServiceCollection AddScoped(this IServiceCollection services)
        {
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<AppSettingsProvider>();

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Add JWT configuration
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            return services;
        }
    }
}
