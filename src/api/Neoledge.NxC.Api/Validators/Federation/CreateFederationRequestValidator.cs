using FluentValidation;
using Neoledge.Nxc.Domain.Api.Federation;

namespace Neoledge.NxC.Api.Validators.Federation
{
    public class CreateFederationRequestValidator : AbstractValidator<CreateFederationParameters>
    {
        public CreateFederationRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}