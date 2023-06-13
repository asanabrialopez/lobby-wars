using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.Application.DTOs
{
    public class LoginResponseDto
    {
        public LoginResponseDto(string? accessToken) 
        {
            AccessToken = accessToken;
        }

        public string? AccessToken { get; set; }
    }
}
