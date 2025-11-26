using MediatR;
using Kedu.Domain.Enums;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.PlanoDePagamento;

public class CreatePlanoDePagamentoCommand : IRequest<Result<PlanoDePagamentoResponse>>
{
    public int ResponsavelId { get; set; }
    public int CentroDeCusto { get; set; }
    public List<CobrancaData> Cobrancas { get; set; } = [];
}

public class CobrancaData
{
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
}