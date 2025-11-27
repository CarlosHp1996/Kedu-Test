using FluentValidation;
using Kedu.Application.Commands.CentroDeCusto;

namespace Kedu.Application.Validators;

public class DeleteCentroDeCustoCommandValidator : AbstractValidator<DeleteCentroDeCustoCommand>
{
    public DeleteCentroDeCustoCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");
    }
}