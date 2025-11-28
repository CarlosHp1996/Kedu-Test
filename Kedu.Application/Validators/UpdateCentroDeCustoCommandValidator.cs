using FluentValidation;
using Kedu.Application.Commands.CentroDeCusto;
using Kedu.Domain.Enums;

namespace Kedu.Application.Validators;

public class UpdateCentroDeCustoCommandValidator : AbstractValidator<UpdateCentroDeCustoCommand>
{
    public UpdateCentroDeCustoCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(2, 100)
            .WithMessage("Nome deve ter entre 2 e 100 caracteres");
    }
}