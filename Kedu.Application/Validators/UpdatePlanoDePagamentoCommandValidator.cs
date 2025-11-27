using FluentValidation;
using Kedu.Application.Commands.PlanoDePagamento;

namespace Kedu.Application.Validators;

public class UpdatePlanoDePagamentoCommandValidator : AbstractValidator<UpdatePlanoDePagamentoCommand>
{
    public UpdatePlanoDePagamentoCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");

        RuleFor(x => x.CentroDeCustoId)
            .GreaterThan(0)
            .WithMessage("CentroDeCustoId deve ser maior que zero");
    }
}