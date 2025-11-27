using FluentValidation;
using Kedu.Application.Commands.CentroDeCusto;
using Kedu.Domain.Enums;

namespace Kedu.Application.Validators;

public class CreateCentroDeCustoCommandValidator : AbstractValidator<CreateCentroDeCustoCommand>
{
    public CreateCentroDeCustoCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(2, 100)
            .WithMessage("Nome deve ter entre 2 e 100 caracteres");

        RuleFor(x => x.Tipo)
            .Must(tipo => Enum.IsDefined(typeof(TipoCentroDeCusto), tipo))
            .WithMessage("Tipo de centro de custo deve ser MATRICULA, MENSALIDADE ou MATERIAL");
    }
}