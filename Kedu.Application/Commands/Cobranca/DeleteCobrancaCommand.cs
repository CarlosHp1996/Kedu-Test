using MediatR;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Cobranca;

public class DeleteCobrancaCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}