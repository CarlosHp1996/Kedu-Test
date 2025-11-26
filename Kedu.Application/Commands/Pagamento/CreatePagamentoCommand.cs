using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Pagamento;

public class CreatePagamentoCommand : IRequest<Result<RegistrarPagamentoResponse>>
{
    public int CobrancaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataPagamento { get; set; }
}