using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Pagamento;

public class GetPagamentosByCobrancaIdQuery : IRequest<Result<List<PagamentoResponse>>>
{
    public int CobrancaId { get; set; }
}