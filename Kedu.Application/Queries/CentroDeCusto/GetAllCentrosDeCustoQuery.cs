using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.CentroDeCusto;

public class GetAllCentrosDeCustoQuery : IRequest<Result<List<CentroDeCustoResponse>>>
{
}