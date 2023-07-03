using FluentValidation;

namespace LobbyWars.API.Features.Contract.Application
{
    /// <summary>
    /// This class validates an EvaluateContractCommand.
    /// </summary>
    public class EvaluateContractValidator : AbstractValidator<EvaluateContractCommand>
    {
        public EvaluateContractValidator()
        {
            RuleFor(r => r.PlaintiffSignatures)
                .NotEmpty()
                .Length(3);
            RuleFor(r => r.DefendantSignatures)
                .NotEmpty()
                .Length(3);

            RuleFor(r => r.DefendantSignatures)
                .NotEmpty()
                .When(m => m.PlaintiffSignatures.Contains("#") && m.DefendantSignatures.Contains("#"))
                .WithMessage("Only one sign can be missing");

        }
    }
}
