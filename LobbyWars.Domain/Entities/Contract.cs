using LobbyWars.Domain.Events;
using LobbyWars.SharedKernel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LobbyWars.Domain.Entities
{
    public class Contract : IEntity
    {
        public Contract(string plaintiffSignatures, string defendantSignatures)
        {
            PlaintiffSignatures = plaintiffSignatures;
            DefendantSignatures = defendantSignatures;
        }

        /// <summary>
        /// Gets or sets the signatures of the plaintiff.
        /// </summary>
        public string PlaintiffSignatures { get; set; }

        /// <summary>
        /// Gets or sets the signatures of the defendant.
        /// </summary>
        public string DefendantSignatures { get; set; }

        public int GetScorePlaintiff => CalculatePoints(PlaintiffSignatures);
        public int GetScoreDefendant => CalculatePoints(DefendantSignatures);

        public int Id { get; set; }

        /// <summary>
        /// Determines the winner of a contract based on the points of the plaintiff's and defendant's signatures.
        /// </summary>
        /// <param name="contract">Contract of plaintiff and defendant.</param>
        /// <returns>A string indicating the winner ("Plaintiff", "Defendant", or "Draw").</returns>
        public async Task<string?> DetermineWinner()
        {
            string? winner = null;
            if (GetScorePlaintiff > GetScoreDefendant)
            {
                winner = ContractConstants.PLAINTIFF;
            }
            else if (GetScoreDefendant > GetScorePlaintiff)
            {
                winner = ContractConstants.DEFENDANT;
            }

            return winner;
        }

        public async Task<char?> DetermineMissingSignatures()
        {
            char? result = null;

            if (PlaintiffSignatures.Contains("#") || DefendantSignatures.Contains("#"))
            {
                result = CalculateSigns();
            }

            return result;
        }

        private char? CalculateSigns()
        {
            char? result = null;
            var plaintiffPoints = CalculatePoints(PlaintiffSignatures);
            var defendantPoints = CalculatePoints(DefendantSignatures);

            int lowerPoints, higherPoints;
            if (plaintiffPoints <= defendantPoints)
            {
                lowerPoints = plaintiffPoints;
                higherPoints = defendantPoints;

                if (DefendantSignatures.Contains("#"))
                {
                    return null;
                }
            }
            else
            {
                lowerPoints = defendantPoints;
                higherPoints = plaintiffPoints;

                if (PlaintiffSignatures.Contains("#"))
                {
                    return null;
                }
            }

            var rest = (higherPoints - lowerPoints);
            if (rest < ContractConstants.VALIDATOR_SCORE)
            {
                result = ContractConstants.VALIDATOR;
            }
            else if (rest < ContractConstants.NOTARY_SCORE)
            {
                result = ContractConstants.NOTARY;
            }
            else
            {
                result = ContractConstants.KING;
            }

            return result;
        }

        /// <summary>
        /// Calculates the points of a string of signatures.
        /// </summary>
        /// <param name="signatures">The string of signatures.</param>
        /// <returns>The total points of the signatures.</returns>
        private int CalculatePoints(string signatures)
        {
            var hasSignatureKing = signatures.Contains(ContractConstants.KING);
            var points = 0;

            foreach (var signature in signatures)
            {
                switch (signature)
                {
                    case ContractConstants.KING:
                        points += ContractConstants.KING_SCORE;
                        break;
                    case ContractConstants.NOTARY:
                        points += ContractConstants.NOTARY_SCORE;
                        break;
                    case ContractConstants.VALIDATOR:
                        if (!hasSignatureKing)
                            points += ContractConstants.VALIDATOR_SCORE;
                        break;
                }
            }

            return points;
        }
    }
}
