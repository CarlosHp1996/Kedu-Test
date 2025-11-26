using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Cobranca;

public class GetCobrancaByIdQuery : IRequest<Result<CobrancaDetailResponse>>
{
    public int Id { get; set; }
}