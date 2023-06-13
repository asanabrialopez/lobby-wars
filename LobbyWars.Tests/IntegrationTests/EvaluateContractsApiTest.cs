using LobbyWars.API.Features.Commands;
using LobbyWars.Application.DTOs.Contracts;
using LobbyWars.SharedKernel.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.Tests.IntegrationTests
{
    public class EvaluateContractsApiTest
    {
        private HttpClient _httpClient;
        private const string REQUEST_URI = "api/contract";

        [SetUp]
        public void Setub()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract1Wins()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "KNV";
            value.PlaintiffSignatures = "NNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<ContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.DEFENDANT, result.Winner);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract2Wins()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NNV";
            value.PlaintiffSignatures = "KNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<ContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.PLAINTIFF, result.Winner);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenTie()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "KNV";
            value.PlaintiffSignatures = "KNV";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<ContractResponseDto>().Result;

            // The method returns null in case of a tie.
            Assert.Null(result.Winner);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNotaryIsRequired()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "N#V";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<ContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.NOTARY, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenKingIsRequired()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "V#V";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<ContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.KING, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenValidatorIsRequired()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "NN#";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<ContractResponseDto>().Result;
            Assert.AreEqual(ContractConstants.VALIDATOR, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNoSignatureIsRequired()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "NVV";
            value.PlaintiffSignatures = "KN#";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            postResponse.EnsureSuccessStatusCode();
            var result = postResponse.Content.ReadFromJsonAsync<ContractResponseDto>().Result;

            // The method returns null if no signature is required.
            Assert.Null(result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenUnknownContract()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "";
            value.PlaintiffSignatures = "";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            Assert.AreEqual(postResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenWhitespaceContract()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = " ";
            value.PlaintiffSignatures = " ";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            Assert.AreEqual(postResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenMissingSign()
        {
            var value = new EvaluateContracts.EvaluateContractCommand();
            value.DefendantSignatures = "KNN";
            value.PlaintiffSignatures = "KN";

            var postResponse = _httpClient.PostAsJsonAsync(REQUEST_URI, value).Result;

            // Assert
            Assert.AreEqual(postResponse.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}
