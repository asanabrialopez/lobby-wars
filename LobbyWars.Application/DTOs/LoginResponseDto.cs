namespace LobbyWars.Application.DTOs
{
    /// <summary>
    /// This class is a Data Transfer Object (DTO) used to encapsulate data for the response to a login request.
    /// </summary>
    public class LoginResponseDto
    {
        public LoginResponseDto(string? accessToken) 
        {
            AccessToken = accessToken;
        }

        /// <summary>
        /// The access token property, nullable string.
        /// </summary>
        public string? AccessToken { get; set; }
    }
}
