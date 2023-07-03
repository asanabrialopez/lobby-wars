namespace LobbyWars.API.Features.Contract.Application
{
    public class EvaluateContractResponseDto
    {
        public EvaluateContractResponseDto(string winner, char? missingSignatures)
        {
            Winner = winner;
            MissingSignatures = missingSignatures;
        }

        /// <summary>
        /// Gets or sets the winner of the contract.
        /// </summary>
        public string? Winner { get; set; }

        /// <summary>
        /// Gets or sets the missing signatures.
        /// </summary>
        public char? MissingSignatures { get; set; }
    }
}
