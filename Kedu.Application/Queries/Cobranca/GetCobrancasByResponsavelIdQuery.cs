using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Cobranca;

public class GetCobrancasByResponsavelIdQuery : IRequest<Result<CobrancaListResponse>>
{
    public int ResponsavelId { get; set; }
}