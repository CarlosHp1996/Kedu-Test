using MediatR;
using Kedu.Domain.Enums;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Cobranca;

public class UpdateCobrancaCommand : IRequest<Result<CobrancaResponse>>
{
    public int Id { get; set; }
    public decimal? Valor { get; set; }
    public DateTime? DataVencimento { get; set; }
    public MetodoPagamento? MetodoPagamento { get; set; }
}