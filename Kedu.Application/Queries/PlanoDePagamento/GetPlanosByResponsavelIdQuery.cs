using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.PlanoDePagamento;

public class GetPlanosByResponsavelIdQuery : IRequest<Result<List<PlanoDePagamentoResponse>>>
{
    public int ResponsavelId { get; set; }
}