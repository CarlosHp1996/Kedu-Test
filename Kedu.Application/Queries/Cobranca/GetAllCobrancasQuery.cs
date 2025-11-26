using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Cobranca;

public class GetAllCobrancasQuery : IRequest<Result<List<CobrancaResponse>>>
{
}