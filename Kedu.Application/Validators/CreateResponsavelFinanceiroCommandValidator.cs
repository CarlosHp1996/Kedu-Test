using FluentValidation;
using Kedu.Application.Commands.ResponsavelFinanceiro;

namespace Kedu.Application.Validators;

public class CreateResponsavelFinanceiroCommandValidator : AbstractValidator<CreateResponsavelFinanceiroCommand>
{
    public CreateResponsavelFinanceiroCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(2, 255)
            .WithMessage("Nome deve ter entre 2 e 255 caracteres");
    }
}