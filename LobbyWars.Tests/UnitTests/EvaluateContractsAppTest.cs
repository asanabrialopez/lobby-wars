using LobbyWars.API.Features.Commands;
using LobbyWars.Application.DTOs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.TestHelper;
using System.Diagnostics.Contracts;
using System.Reflection;
using LobbyWars.SharedKernel.Constants;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using LobbyWars.Application.Interfaces;
using LobbyWars.Application.Services;
using LobbyWars.Domain.Entities;

namespace LobbyWars.Tests.UnitTests
{
    public class ContractModuleTests
    {
        private IContractService _service;

        [SetUp]
        public void Setub()
        {
            _service = new ContractService();
        }


        [Test]
        public void TestContractWinnerDetermination_WhenContract1Wins()
        {
            var value = new ContractEntity("NNV", "KNV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.DEFENDANT, result.Winner);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract2Wins()
        {
            var value = new ContractEntity("KNV", "NNV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.PLAINTIFF, result.Winner);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenTie()
        {
            var value = new ContractEntity("KNV", "KNV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert


            // The method returns null in case of a tie.
            Assert.Null(result.Winner);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNotaryIsRequired()
        {
            var value = new ContractEntity("N#V", "NVV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.NOTARY, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenKingIsRequired()
        {
            var value = new ContractEntity("V#V", "NVV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.KING, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenValidatorIsRequired()
        {
            var value = new ContractEntity("NN#", "NVV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.VALIDATOR, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNoSignatureIsRequired()
        {
            var value = new ContractEntity("KN#", "NVV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            // The method returns null if no signature is required.
            Assert.Null(result.MissingSignatures);
        }
    }
}
