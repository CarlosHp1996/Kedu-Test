using FluentValidation;
using Kedu.Application.Commands.PlanoDePagamento;

namespace Kedu.Application.Validators;

public class DeletePlanoDePagamentoCommandValidator : AbstractValidator<DeletePlanoDePagamentoCommand>
{
    public DeletePlanoDePagamentoCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");
    }
}