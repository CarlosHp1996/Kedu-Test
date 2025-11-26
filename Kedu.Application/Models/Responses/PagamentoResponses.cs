using Kedu.Domain.Enums;

namespace Kedu.Application.Models.Responses;

public class PagamentoResponse
{
    public int Id { get; set; }
    public int CobrancaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
}

public class PagamentoDetailResponse
{
    public int Id { get; set; }
    public int CobrancaId { get; set; }
    public CobrancaResponse? Cobranca { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
}

public class RegistrarPagamentoResponse
{
    public int PagamentoId { get; set; }
    public int CobrancaId { get; set; }
    public StatusCobranca NovoStatusCobranca { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public bool Sucesso { get; set; }
}