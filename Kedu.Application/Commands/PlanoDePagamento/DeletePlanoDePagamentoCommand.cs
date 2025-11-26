using MediatR;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.PlanoDePagamento;

public class DeletePlanoDePagamentoCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}