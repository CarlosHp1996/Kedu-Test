namespace Kedu.Application.Models.Responses;

public class ResponsavelFinanceiroResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class ResponsavelFinanceiroDetailResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public List<PlanoDePagamentoSummaryResponse> PlanosDePagamento { get; set; } = [];
    public int TotalPlanos { get; set; }
    public decimal ValorTotalPlanos { get; set; }
}

public class PlanoDePagamentoSummaryResponse
{
    public int Id { get; set; }
    public string CentroDeCustoNome { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public int QuantidadeCobrancas { get; set; }
}