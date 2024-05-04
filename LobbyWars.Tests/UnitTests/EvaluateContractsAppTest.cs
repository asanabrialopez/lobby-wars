using LobbyWars.API.Features.Contract.Application;
using LobbyWars.API.Features.Contract.Domain;
using LobbyWars.SharedKernel.Constants;

namespace LobbyWars.Tests.UnitTests
{
    public class ContractModuleTests
    {
        private IEvaluateContractService _evaluateContract;

        [SetUp]
        public void Setub()
        {
            _evaluateContract = new EvaluateContractService();
        }


        [Test]
        public void TestContractWinnerDetermination_WhenContract1Wins()
        {
            var value = new ContractEntity("NNV", "KNV");

            var result = _evaluateContract.Invoke(value).Result;

            // Assert
            Assert.That(ContractConstants.DEFENDANT, Is.EqualTo(result.Winner));
        }

        [Test]
        public void TestContractWinnerDetermination_WhenContract2Wins()
        {
            var value = new ContractEntity("KNV", "NNV");

            var result = _evaluateContract.Invoke(value).Result;

            // Assert
            Assert.That(ContractConstants.PLAINTIFF, Is.EqualTo(result.Winner));
        }

        [Test]
        public void TestContractWinnerDetermination_WhenTie()
        {
            var value = new ContractEntity("KNV", "KNV");

            var result = _evaluateContract.Invoke(value).Result;

            // Assert
            // The method returns null in case of a tie.
            Assert.That(result.Winner, Is.Null);
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNotaryIsRequired()
        {
            var value = new ContractEntity("N#V", "NVV");

            var result = _evaluateContract.Invoke(value).Result;

            // Assert
            Assert.That(ContractConstants.NOTARY, Is.EqualTo(result.MissingSignatures));
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenKingIsRequired()
        {
            var value = new ContractEntity("V#V", "NVV");

            var result = _evaluateContract.Invoke(value).Result;

            // Assert
            Assert.That(ContractConstants.KING, Is.EqualTo(result.MissingSignatures));
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenValidatorIsRequired()
        {
            var value = new ContractEntity("NN#", "NVV");

            var result = _evaluateContract.Invoke(value).Result;

            // Assert
            Assert.That(ContractConstants.VALIDATOR, Is.EqualTo(result.MissingSignatures));
        }

        [Test]
        public void TestMinimumSignatureDetermination_WhenNoSignatureIsRequired()
        {
            var value = new ContractEntity("KN#", "NVV");

            var result = _evaluateContract.Invoke(value).Result;

            // Assert
            // The method returns null if no signature is required.
            Assert.That(result.MissingSignatures, Is.Null);
        }
    }
}
