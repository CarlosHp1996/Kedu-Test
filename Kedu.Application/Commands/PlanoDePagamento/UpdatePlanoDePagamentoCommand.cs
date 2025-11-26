using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.PlanoDePagamento;

public class UpdatePlanoDePagamentoCommand : IRequest<Result<PlanoDePagamentoResponse>>
{
    public int Id { get; set; }
    public int CentroDeCustoId { get; set; }
}