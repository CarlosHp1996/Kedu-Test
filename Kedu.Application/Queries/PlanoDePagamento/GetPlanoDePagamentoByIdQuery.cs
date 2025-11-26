using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.PlanoDePagamento;

public class GetPlanoDePagamentoByIdQuery : IRequest<Result<PlanoDePagamentoDetailResponse>>
{
    public int Id { get; set; }
}