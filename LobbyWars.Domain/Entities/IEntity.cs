using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LobbyWars.Domain.Entities
{
    public interface IEntity
    {
        
        public int Id { get; set; }
    }
}
