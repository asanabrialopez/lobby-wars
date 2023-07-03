using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LobbyWars.API.Features.Contract.Domain
{
    public class UserEntity : IEntity
    {
        public UserEntity() { }
        public UserEntity(string email, string password) 
        {
            Email = email;
            Password = password;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
