using MediatR;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.CentroDeCusto;

public class DeleteCentroDeCustoCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}