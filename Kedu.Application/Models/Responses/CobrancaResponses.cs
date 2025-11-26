using Kedu.Domain.Enums;

namespace Kedu.Application.Models.Responses;

public class CobrancaResponse
{
    public int Id { get; set; }
    public int PlanoDePagamentoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public string MetodoPagamentoDescricao { get; set; } = string.Empty;
    public StatusCobranca Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public string CodigoPagamento { get; set; } = string.Empty;
    public bool IsVencida { get; set; }
    public int QuantidadePagamentos { get; set; }
}

public class CobrancaDetailResponse
{
    public int Id { get; set; }
    public int PlanoDePagamentoId { get; set; }
    public PlanoDePagamentoSummaryResponse? PlanoDePagamento { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public string MetodoPagamentoDescricao { get; set; } = string.Empty;
    public StatusCobranca Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public string CodigoPagamento { get; set; } = string.Empty;
    public bool IsVencida { get; set; }
    public List<PagamentoResponse> Pagamentos { get; set; } = [];
}

public class CobrancaListResponse
{
    public List<CobrancaDetailResponse> Cobrancas { get; set; } = [];
    public int TotalCount { get; set; }
    public decimal ValorTotal { get; set; }
}