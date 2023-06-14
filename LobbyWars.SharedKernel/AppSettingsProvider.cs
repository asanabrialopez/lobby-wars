using Microsoft.Extensions.Configuration;

namespace LobbyWars.SharedKernel
{
    public class AppSettingsProvider
    {
        public AppSettingsProvider(IConfiguration conf) 
        {
            this.Pepper = conf.GetSection("ApplicationSettings")["Pepper"];
            this.JWTSecret = conf.GetSection("Jwt")["Secret"];
            this.JWTIssuer = conf.GetSection("Jwt")["Issuer"];
            this.JWTAudience = conf.GetSection("Jwt")["Audience"];
        }

        public string Pepper { get; set; }
        public string JWTSecret { get; set; }
        public string JWTIssuer { get; set; }
        public string JWTAudience { get; set; }
    }
}
