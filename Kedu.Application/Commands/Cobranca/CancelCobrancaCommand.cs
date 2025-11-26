using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Cobranca;

public class CancelCobrancaCommand : IRequest<Result<CobrancaResponse>>
{
    public int Id { get; set; }
    public string Motivo { get; set; } = string.Empty;
}