using MediatR;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Cobranca;

public class GetCobrancasQuantidadeByResponsavelIdQuery : IRequest<Result<int>>
{
    public int ResponsavelId { get; set; }
}