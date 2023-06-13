using LobbyWars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.Infrastructure.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {

        IQueryable<TEntity> GetAll();

        Task<TEntity?> GetById(int id);

        Task Create(TEntity entity);

        Task Update(int id, TEntity entity);

        Task Delete(int id);
    }
}
