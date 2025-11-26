using Kedu.Domain.Enums;

namespace Kedu.Application.Models.Requests;

public class CreatePlanoDePagamentoRequest
{
    public int ResponsavelId { get; set; }
    public int CentroDeCusto { get; set; }
    public List<CobrancaRequest> Cobrancas { get; set; } = [];
}

public class UpdatePlanoDePagamentoRequest
{
    public int CentroDeCustoId { get; set; }
}

public class CobrancaRequest
{
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
}