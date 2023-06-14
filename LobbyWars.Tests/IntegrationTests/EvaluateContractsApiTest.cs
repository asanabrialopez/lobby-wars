using LobbyWars.API.Commands;
using LobbyWars.API.Features.Commands;
using LobbyWars.Application.DTOs;
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

            var value = new Login.LoginCommand();
            value.Email = "king@lobbywars.com";
            value.Password = "king";
            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_USER, value).Result;
            var result = postResponse.Content.ReadFromJsonAsync<LoginResponseDto>().Result;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract1Wins()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "KNV";
            value.PlaintiffSignatures = "NNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.DEFENDANT, result.Winner);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract2Wins()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NNV";
            value.PlaintiffSignatures = "KNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.PLAINTIFF, result.Winner);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenTie()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "KNV";
            value.PlaintiffSignatures = "KNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;

            // The method returns null in case of a tie.
            Assert.Null(result.Winner);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNotaryIsRequired()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "N#V";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.NOTARY, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenKingIsRequired()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "V#V";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.KING, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenValidatorIsRequired()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "NN#";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.VALIDATOR, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNoSignatureIsRequired()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "KN#";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<EvaluateContractResponseDto>().Result;

            // The method returns null if no signature is required.
            Assert.Null(result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenUnknownContract()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "";
            value.PlaintiffSignatures = "";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            Assert.AreEqual(postResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenWhitespaceContract()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = " ";
            value.PlaintiffSignatures = " ";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            Assert.AreEqual(postResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenMissingSign()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "KNN";
            value.PlaintiffSignatures = "KN";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI_CONTRACT, value).Result;

            // Assert
            Assert.AreEqual(postResponse.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}
