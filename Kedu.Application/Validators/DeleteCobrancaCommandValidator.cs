using FluentValidation;
using Kedu.Application.Commands.Cobranca;

namespace Kedu.Application.Validators;

public class DeleteCobrancaCommandValidator : AbstractValidator<DeleteCobrancaCommand>
{
    public DeleteCobrancaCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");
    }
}