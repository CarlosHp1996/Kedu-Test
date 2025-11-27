using FluentValidation;
using Kedu.Application.Commands.ResponsavelFinanceiro;

namespace Kedu.Application.Validators;

public class DeleteResponsavelFinanceiroCommandValidator : AbstractValidator<DeleteResponsavelFinanceiroCommand>
{
    public DeleteResponsavelFinanceiroCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");
    }
}