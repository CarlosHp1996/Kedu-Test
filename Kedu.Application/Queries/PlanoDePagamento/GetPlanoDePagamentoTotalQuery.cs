using MediatR;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.PlanoDePagamento;

public class GetPlanoDePagamentoTotalQuery : IRequest<Result<decimal>>
{
    public int Id { get; set; }
}