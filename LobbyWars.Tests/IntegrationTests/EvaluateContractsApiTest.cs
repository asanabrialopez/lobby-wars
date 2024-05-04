using LobbyWars.API.Features.Contract.Api;
using LobbyWars.API.Features.Contract.Application;
using LobbyWars.API.Features.User.Application.Login;
using LobbyWars.API.Modules;
using LobbyWars.SharedKernel.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LobbyWars.Tests.IntegrationTests
{
    public class EvaluateContractsApiTest
    {
        private HttpClient _httpClient;
        private const string REQUEST_URI_CONTRACT = "api/contract";
        private const string REQUEST_URI_USER = "api/user";

        [SetUp]
        public void Setub()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();

            var value = new LoginCommand();
            value.Email = "king@lobbywars.com";
            value.Password = "king";
            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_USER, value).Result;
            var result = postResponse.Content.ReadFromJsonAsync<LoginResponseDto>().Result;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract1Wins()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "KNV";
            value.PlaintiffSignatures = "NNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.That(ContractConstants.DEFENDANT, Is.EqualTo(result.Winner));
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract2Wins()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "NNV";
            value.PlaintiffSignatures = "KNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.That(ContractConstants.PLAINTIFF, Is.EqualTo(result.Winner));
        }

        [Test]
        public void TestContractWinnerDetermination_WhenTie()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "KNV";
            value.PlaintiffSignatures = "KNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;

            // The method returns null in case of a tie.
            Assert.That(result.Winner, Is.Null);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNotaryIsRequired()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "N#V";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.That(ContractConstants.NOTARY, Is.EqualTo(result.MissingSignatures));
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenKingIsRequired()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "V#V";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.That(ContractConstants.KING, Is.EqualTo(result.MissingSignatures));
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenValidatorIsRequired()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "NN#";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.That(ContractConstants.VALIDATOR, Is.EqualTo(result.MissingSignatures));
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNoSignatureIsRequired()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "KN#";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;

            // The method returns null if no signature is required.
            Assert.That(result.MissingSignatures, Is.Null);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenUnknownContract()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "";
            value.PlaintiffSignatures = "";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenWhitespaceContract()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = " ";
            value.PlaintiffSignatures = " ";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenMissingSign()
        {
            var value = new EvaluateContractCommand();
            value.DefendantSignatures = "KNN";
            value.PlaintiffSignatures = "KN";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
