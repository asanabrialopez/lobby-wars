using LobbyWars.Application.Services;
using LobbyWars.Domain.Repositories;
using LobbyWars.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LobbyWars.Tests.UnitTests
{
    internal class LoginAppTest
    {
        private IUserService _service;

        [SetUp]
        public void Setub()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                 .AddEnvironmentVariables()
                 .Build();

            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "LobbyWars")
               .Options;

            var appSettings = new SharedKernel.AppSettingsProvider(config);
            _service = new UserService(
                repository: new UserRepository(new AppDbContext(options), appSettings),
                appSettings: appSettings);
        }


        [Test]
        public void TestContractLogin_Success()
        {
            var result = _service.Login("king@lobbywars.com", "king").Result;

            // Assert
            Assert.IsNotNull(result.AccessToken);
        }

        [Test]
        public void TestContractLogin_Fail()
        {
            var result = _service.Login("test@lobbywars.com", "test").Result;

            // Assert
            Assert.Null(result.AccessToken);
        }
    }
}
