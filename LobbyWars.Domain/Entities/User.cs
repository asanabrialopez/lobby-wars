using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LobbyWars.Domain.Entities
{
    public class User : IEntity
    {
        public User() { }
        public User(string email, string password) 
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
