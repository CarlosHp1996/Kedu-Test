using FluentValidation;
using Kedu.Application.Commands.ResponsavelFinanceiro;

namespace Kedu.Application.Validators;

public class UpdateResponsavelFinanceiroCommandValidator : AbstractValidator<UpdateResponsavelFinanceiroCommand>
{
    public UpdateResponsavelFinanceiroCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(2, 255)
            .WithMessage("Nome deve ter entre 2 e 255 caracteres");
    }
}