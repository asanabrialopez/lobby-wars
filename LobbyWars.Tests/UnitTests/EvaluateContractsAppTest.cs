using LobbyWars.SharedKernel.Constants;
using LobbyWars.Application.Services;

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
            var value = new Domain.Entities.Contract("NNV", "KNV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.DEFENDANT, result.Winner);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract2Wins()
        {
            var value = new Domain.Entities.Contract("KNV", "NNV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.PLAINTIFF, result.Winner);
        }

        [Test]
        public void TestContractWinnerDetermination_WhenTie()
        {
            var value = new Domain.Entities.Contract("KNV", "KNV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            // The method returns null in case of a tie.
            Assert.Null(result.Winner);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNotaryIsRequired()
        {
            var value = new Domain.Entities.Contract("N#V", "NVV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.NOTARY, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenKingIsRequired()
        {
            var value = new Domain.Entities.Contract("V#V", "NVV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.KING, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenValidatorIsRequired()
        {
            var value = new Domain.Entities.Contract("NN#", "NVV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            Assert.AreEqual(ContractConstants.VALIDATOR, result.MissingSignatures);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNoSignatureIsRequired()
        {
            var value = new Domain.Entities.Contract("KN#", "NVV");

            var result = _service.EvaluateContracts(value).Result;

            // Assert
            // The method returns null if no signature is required.
            Assert.Null(result.MissingSignatures);
        }
    }
}
