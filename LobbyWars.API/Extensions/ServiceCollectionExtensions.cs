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
        /// <summary>
        /// Extension method for IServiceCollection to configure Swagger.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Return the service collection for further configuration.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            // Define the OpenAPI Security Scheme which describes how the API is protected.
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JSON Web Token based security",
            };

            // Define the OpenAPI Security Requirement which provides the list of required security schemes for the API.
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

        /// <summary>
        /// Extension method for IServiceCollection to register application services for dependency injection.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Return the service collection for further configuration.</returns>
        public static IServiceCollection AddScoped(this IServiceCollection services)
        {
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<AppSettingsProvider>();

            return services;
        }

        /// <summary>
        /// Extension method for IServiceCollection to add and configure authentication services.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration manager</param>
        /// <returns>Return the service collection for further configuration.</returns>
        public static IServiceCollection AddAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Add and configure JWT authentication.
            services.AddAuthentication(o =>
            {
                // Set default schemes for authentication, challenge, and the default scheme.
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                // Set the parameters for token validation.
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    // Set the valid issuer and audience from configuration.
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],

                    // Set the symmetric security key for signing the token.
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])),

                    // Set validation rules: issuer, audience, lifetime, and issuer signing key should be validated.
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
