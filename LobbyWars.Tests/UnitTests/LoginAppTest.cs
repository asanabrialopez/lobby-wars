using LobbyWars.API.Features.User.Application.Login;
using LobbyWars.Database;
using LobbyWars.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LobbyWars.Tests.UnitTests
{
    internal class LoginAppTest
    {
        private ILoginService _login;

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
            _login = new LoginService(
                repository: new UserRepository(new AppDbContext(options), appSettings),
                appSettings: appSettings);
        }


        [Test]
        public void TestContractLogin_Success()
        {
            var result = _login.Invoke("king@lobbywars.com", "king").Result;

            // Assert
            Assert.That(result.AccessToken, Is.Not.Null);
        }

        [Test]
        public void TestContractLogin_Fail()
        {
            var result = _login.Invoke("test@lobbywars.com", "test").Result;

            // Assert
            Assert.That(result.AccessToken, Is.Null);
        }
    }
}
