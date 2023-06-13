﻿using LobbyWars.Domain.Entities;
using LobbyWars.Domain.Repositories;
using LobbyWars.Infrastructure.Database;
using LobbyWars.Infrastructure.Repositories;
using LobbyWars.SharedKernel;
using LobbyWars.SharedKernel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace LobbyWars.Domain.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext, AppSettingsProvider appSettingsProvider) : base(dbContext)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            using (var context = new AppDbContext(optionsBuilder.Options))
            {
                var user = new User
                {
                    Email = "king@lobbywars.com",
                    PasswordSalt = PasswordHasher.GenerateSalt()
                };
                user.PasswordHash = PasswordHasher.ComputeHash("king", user.PasswordSalt, appSettingsProvider.Pepper);

                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext.Set<User>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
