using MediatR;
using Kedu.Domain.Enums;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.CentroDeCusto;

public class UpdateCentroDeCustoCommand : IRequest<Result<CentroDeCustoResponse>>
{
    public int Id { get; set; }
    public required string Nome { get; set; }
}