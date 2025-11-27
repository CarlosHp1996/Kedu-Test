using FluentValidation;
using Kedu.Application.Commands.Cobranca;

namespace Kedu.Application.Validators;

public class CancelCobrancaCommandValidator : AbstractValidator<CancelCobrancaCommand>
{
    public CancelCobrancaCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");

        RuleFor(x => x.Motivo)
            .NotEmpty()
            .WithMessage("Motivo do cancelamento é obrigatório")
            .Length(3, 500)
            .WithMessage("Motivo deve ter entre 3 e 500 caracteres");
    }
}