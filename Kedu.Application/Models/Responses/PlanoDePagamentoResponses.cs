using Kedu.Domain.Enums;

namespace Kedu.Application.Models.Responses;

public class PlanoDePagamentoResponse
{
    public int Id { get; set; }
    public int ResponsavelFinanceiroId { get; set; }
    public string ResponsavelFinanceiroNome { get; set; } = string.Empty;
    public int CentroDeCustoId { get; set; }
    public string CentroDeCustoNome { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public int QuantidadeCobrancas { get; set; }
    public List<CobrancaResponse> Cobrancas { get; set; } = [];
}

public class PlanoDePagamentoDetailResponse
{
    public int Id { get; set; }
    public ResponsavelFinanceiroResponse ResponsavelFinanceiro { get; set; } = null!;
    public CentroDeCustoResponse CentroDeCusto { get; set; } = null!;
    public decimal ValorTotal { get; set; }
    public int QuantidadeCobrancas { get; set; }
    public List<CobrancaDetailResponse> Cobrancas { get; set; } = [];
}