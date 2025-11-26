using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Pagamento;

public class GetPagamentoByIdQuery : IRequest<Result<PagamentoDetailResponse>>
{
    public int Id { get; set; }
}