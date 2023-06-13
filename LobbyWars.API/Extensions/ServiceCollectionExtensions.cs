using LobbyWars.Application.Interfaces;
using LobbyWars.Application.Services;
using Microsoft.OpenApi.Models;

namespace LobbyWars.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Description = "We are in the era of \"lawsuits\", everyone wants to go to court with their lawyer Saul and try to get a lot of dollars as if they were raining over\r\nManhattan.\r\nThe laws have changed much lately and governments have been digitized. That's when Signaturit comes into play.\r\nThe city council through the use of Signaturit maintains a registry of legal signatures of each party involved in the contracts that are made.\r\nDuring a trial, justice only verifies the signatures of the parties involved in the contract to decide who wins. For that, they assign points to the\r\ndifferent signatures depending on the role of who signed.",
                    Title = "Lobby Wars API",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Alexis Sanabria",
                        Url = new Uri("https://www.linkedin.com/in/alexis-sanabria-lopez/"),
                        Email = "asanabrial@outlook.com"
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddScoped(this IServiceCollection services)
        {
            services.AddScoped<IContractService, ContractService>();

            return services;
        }
    }
}
