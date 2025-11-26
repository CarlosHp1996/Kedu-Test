using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.CentroDeCusto;

public class GetCentroDeCustoByIdQuery : IRequest<Result<CentroDeCustoResponse>>
{
    public int Id { get; set; }
}